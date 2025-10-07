document.addEventListener('DOMContentLoaded', function () {
    const findAccountBtn = document.getElementById('findAccountBtn');
    const closeAccountBtn = document.getElementById('closeAccountBtn');
    const confirmationCheckbox = document.getElementById('confirmationCheckbox');
    const accountInfo = document.getElementById('accountInfo');
    const requirementsList = document.getElementById('requirementsList');
    const closeAccountSection = document.getElementById('closeAccountSection');
    const warningMessage = document.getElementById('warningMessage');
    const loading = document.getElementById('loading');
    const successMessage = document.getElementById('successMessage');

    const accountNumberInput = document.getElementById('accountNumber');
    const accountDetails = document.getElementById('accountDetails');
    const requirementsItems = document.getElementById('requirementsItems');

    let currentAccount = null;

    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
        return null;
    }

    async function makeAuthenticatedRequest(url, method = 'GET', data = null) {
        const token = getCookie('jwt');

        if (!token) {
            throw new Error('JWT token not found');
        }

        const options = {
            method: method,
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        };

        if (data) {
            options.body = JSON.stringify(data);
        }

        const response = await fetch(url, options);

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        return await response.json();
    }

    accountNumberInput.addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');

        if (value.length > 20) {
            value = value.substring(0, 20);
        }

        if (value.length > 0) {
            value = value.replace(/(\d{4})/g, '$1 ').trim();
        }

        e.target.value = value;
    });

    function validateAccountNumber(number) {
        const cleanNumber = number.replace(/\s/g, '');
        return cleanNumber.length === 16 && /^\d+$/.test(cleanNumber);
    }

    function checkAccountClosable(account) {
        const requirements = [];

        if (account.balance !== 0) {
            requirements.push('Баланс счета должен быть равен нулю');
        }

        if (account.hasActiveLoans) {
            requirements.push('Отсутствие активных кредитов, привязанных к счету');
        }

        if (account.hasActiveDeposits) {
            requirements.push('Отсутствие активных вкладов, привязанных к счету');
        }

        if (account.hasPendingTransactions) {
            requirements.push('Отсутствие pending транзакций');
        }

        return {
            closable: requirements.length === 0,
            requirements: requirements
        };
    }

    function displayAccountInfo(account) {
        const balanceClass = account.balance > 0 ? 'balance-positive' :
            account.balance < 0 ? 'balance-negative' : 'balance-zero';

        accountDetails.innerHTML = `
                    <div class="account-detail">
                        <strong>Номер счета:</strong>
                        <span>${account.accountNumber}</span>
                    </div>
                    <div class="account-detail">
                        <strong>Тип счета:</strong>
                        <span>${account.accountType}</span>
                    </div>
                    <div class="account-detail">
                        <strong>Баланс:</strong>
                        <span class="${balanceClass}">${account.balance.toLocaleString('ru-RU')} ${account.currency}</span>
                    </div>
                    <div class="account-detail">
                        <strong>Валюта:</strong>
                        <span>${account.currency}</span>
                    </div>
                    <div class="account-detail">
                        <strong>Статус:</strong>
                        <span>${account.status}</span>
                    </div>
                    <div class="account-detail">
                        <strong>Дата открытия:</strong>
                        <span>${new Date(account.openDate).toLocaleDateString('ru-RU')}</span>
                    </div>
                `;

        accountInfo.style.display = 'block';
    }

    function displayRequirements(requirements) {
        if (requirements.length > 0) {
            requirementsItems.innerHTML = requirements.map(req =>
                `<li>${req}</li>`
            ).join('');
            requirementsList.style.display = 'block';
            closeAccountSection.style.display = 'none';
            warningMessage.style.display = 'block';
            warningMessage.textContent = 'Невозможно закрыть счет. Выполните следующие требования:';
        } else {
            requirementsList.style.display = 'none';
            closeAccountSection.style.display = 'block';
            warningMessage.style.display = 'none';
        }
    }

    findAccountBtn.addEventListener('click', async function () {
        const accountNumber = accountNumberInput.value;

        if (!validateAccountNumber(accountNumber)) {
            document.getElementById('accountNumberError').style.display = 'block';
            return;
        }

        document.getElementById('accountNumberError').style.display = 'none';
        loading.style.display = 'block';

        try {
            const cleanAccountNumber = accountNumber.replace(/\s/g, '');

            const response = await fetch('/deleteAccount', {
                method: "DELETE",
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Id: cleanAccountNumber,
                })
            });

            if (!response.ok) {
                alert('Не удалось найти счет по данном Id!');
            }
            const responseData = await response.json();

            displayAccountInfo(responseData);

            const closableCheck = checkAccountClosable(accountData);
            displayRequirements(closableCheck.requirements);

            loading.style.display = 'none';

            //setTimeout(() => {


            //    const cleanAccountNumber = accountNumber.replace(/\s/g, '');
            //    const accountData = {
            //        id: 'acc-' + cleanAccountNumber.slice(-4),
            //        accountNumber: accountNumber,
            //        accountType: 'Расчетный',
            //        balance: 0,
            //        currency: 'RUB',
            //        status: 'Активный',
            //        openDate: '2023-01-15',
            //        hasActiveLoans: false,
            //        hasActiveDeposits: false,
            //        hasPendingTransactions: false
            //    };

            //    currentAccount = accountData;

            //    displayAccountInfo(accountData);

            //    const closableCheck = checkAccountClosable(accountData);
            //    displayRequirements(closableCheck.requirements);

            //    loading.style.display = 'none';
            //}, 1000);

        } catch (error) {
            console.error('Error:', error);
            loading.style.display = 'none';
            alert('Ошибка при поиске счета: ' + error.message);
        }
    });

    confirmationCheckbox.addEventListener('change', function () {
        closeAccountBtn.disabled = !this.checked;
    });

    closeAccountBtn.addEventListener('click', async function () {
        const reason = document.getElementById('closingReason').value;

        if (!reason) {
            document.getElementById('reasonError').style.display = 'block';
            return;
        }

        if (!confirmationCheckbox.checked) {
            document.getElementById('confirmationError').style.display = 'block';
            return;
        }

        document.getElementById('reasonError').style.display = 'none';
        document.getElementById('confirmationError').style.display = 'none';

        loading.style.display = 'block';

        try {
            setTimeout(() => {


                loading.style.display = 'none';
                closeAccountSection.style.display = 'none';
                successMessage.style.display = 'block';
                successMessage.innerHTML = `
                            Счет успешно закрыт!<br>
                            Номер счета: <strong>${currentAccount.accountNumber}</strong><br>
                            Дата закрытия: <strong>${new Date().toLocaleDateString('ru-RU')}</strong>
                        `;

            }, 2000);

        } catch (error) {
            console.error('Error:', error);
            loading.style.display = 'none';
            alert('Ошибка при закрытии счета: ' + error.message);
        }
    });
});