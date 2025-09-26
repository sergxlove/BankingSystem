document.addEventListener('DOMContentLoaded', function () {
    const findDepositBtn = document.getElementById('findDepositBtn');
    const closeDepositBtn = document.getElementById('closeDepositBtn');
    const confirmationCheckbox = document.getElementById('confirmationCheckbox');
    const depositInfo = document.getElementById('depositInfo');
    const requirementsList = document.getElementById('requirementsList');
    const closeDepositSection = document.getElementById('closeDepositSection');
    const warningMessage = document.getElementById('warningMessage');
    const infoMessage = document.getElementById('infoMessage');
    const loading = document.getElementById('loading');
    const successMessage = document.getElementById('successMessage');
    const calculationResults = document.getElementById('calculationResults');
    const penaltyWarning = document.getElementById('penaltyWarning');

    const depositNumberInput = document.getElementById('depositNumber');
    const depositDetails = document.getElementById('depositDetails');
    const requirementsItems = document.getElementById('requirementsItems');
    const destinationAccountSelect = document.getElementById('destinationAccount');

    let currentDeposit = null;
    let clientAccounts = [];

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

    function checkDepositClosable(deposit) {
        const requirements = [];

        if (deposit.isBlocked) {
            requirements.push('Вклад не должен быть заблокирован');
        }

        if (deposit.isClosed) {
            requirements.push('Вклад уже закрыт');
        }

        return {
            closable: requirements.length === 0,
            requirements: requirements
        };
    }

    function calculateClosingAmount(deposit, isEarly = false) {
        const principal = deposit.amount;
        const interestRate = deposit.interestRate;
        const startDate = new Date(deposit.startDate);
        const endDate = new Date(deposit.endDate);
        const currentDate = new Date();

        const totalDays = Math.floor((endDate - startDate) / (1000 * 60 * 60 * 24));
        const passedDays = Math.floor((currentDate - startDate) / (1000 * 60 * 60 * 24));

        let interest = 0;
        let penalty = 0;

        if (isEarly) {
            const demandRate = 0.01;
            interest = principal * demandRate * (passedDays / 365);
            penalty = principal * (interestRate / 100) * (passedDays / 365) - interest;
        } else {
            interest = principal * (interestRate / 100) * (totalDays / 365);
        }

        const totalAmount = principal + interest - penalty;

        return {
            principal: principal,
            interest: interest,
            penalty: penalty,
            totalAmount: totalAmount,
            isEarly: isEarly,
            passedDays: passedDays,
            totalDays: totalDays
        };
    }

    function displayDepositInfo(deposit) {
        const today = new Date();
        const endDate = new Date(deposit.endDate);
        const isMatured = today >= endDate;

        depositDetails.innerHTML = `
                    <div class="deposit-detail">
                        <strong>Номер вклада:</strong>
                        <span>${deposit.depositNumber}</span>
                    </div>
                    <div class="deposit-detail">
                        <strong>Тип вклада:</strong>
                        <span>${deposit.depositType}</span>
                    </div>
                    <div class="deposit-detail">
                        <strong>Сумма вклада:</strong>
                        <span class="positive">${deposit.amount.toLocaleString('ru-RU')} руб.</span>
                    </div>
                    <div class="deposit-detail">
                        <strong>Процентная ставка:</strong>
                        <span>${deposit.interestRate}% годовых</span>
                    </div>
                    <div class="deposit-detail">
                        <strong>Срок вклада:</strong>
                        <span>${deposit.termMonths} месяцев</span>
                    </div>
                    <div class="deposit-detail">
                        <strong>Дата открытия:</strong>
                        <span>${new Date(deposit.startDate).toLocaleDateString('ru-RU')}</span>
                    </div>
                    <div class="deposit-detail">
                        <strong>Дата окончания:</strong>
                        <span>${new Date(deposit.endDate).toLocaleDateString('ru-RU')}</span>
                    </div>
                    <div class="deposit-detail">
                        <strong>Статус:</strong>
                        <span class="${isMatured ? 'positive' : 'warning'}">${isMatured ? 'Срок истек' : 'Действующий'}</span>
                    </div>
                `;

        depositInfo.style.display = 'block';

        if (isMatured) {
            infoMessage.style.display = 'block';
            infoMessage.textContent = 'Срок вклада истек. Вы можете закрыть вклад без штрафных санкций.';
        } else {
            infoMessage.style.display = 'block';
            infoMessage.textContent = 'Вклад действующий. При досрочном закрытии могут применяться штрафные санкции.';
        }
    }

    function displayCalculations(calculation) {
        document.getElementById('principalAmount').textContent = `${calculation.principal.toLocaleString('ru-RU')} руб.`;
        document.getElementById('interestAmount').textContent = `${calculation.interest.toLocaleString('ru-RU')} руб.`;
        document.getElementById('penaltyAmount').textContent = `${calculation.penalty.toLocaleString('ru-RU')} руб.`;
        document.getElementById('totalAmount').textContent = `${calculation.totalAmount.toLocaleString('ru-RU')} руб.`;

        calculationResults.style.display = 'block';

        if (calculation.isEarly && calculation.penalty > 0) {
            document.getElementById('penaltyPercent').textContent = '50%';
            penaltyWarning.style.display = 'block';
        } else {
            penaltyWarning.style.display = 'none';
        }
    }

    function displayRequirements(requirements) {
        if (requirements.length > 0) {
            requirementsItems.innerHTML = requirements.map(req =>
                `<li>${req}</li>`
            ).join('');
            requirementsList.style.display = 'block';
            closeDepositSection.style.display = 'none';
            warningMessage.style.display = 'block';
            warningMessage.textContent = 'Невозможно закрыть вклад. Выполните следующие требования:';
        } else {
            requirementsList.style.display = 'none';
            closeDepositSection.style.display = 'block';
            warningMessage.style.display = 'none';
        }
    }

    async function loadClientAccounts(clientId) {
        try {
            const accounts = [
                { id: 'acc-001', number: '4081 7810 0000 0000 0001', balance: 15000, currency: 'RUB' },
                { id: 'acc-002', number: '4081 7810 0000 0000 0002', balance: 50000, currency: 'RUB' }
            ];

            destinationAccountSelect.innerHTML = '<option value="">Выберите счет</option>' +
                accounts.map(acc =>
                    `<option value="${acc.id}">${acc.number} (${acc.balance.toLocaleString('ru-RU')} ${acc.currency})</option>`
                ).join('');

            clientAccounts = accounts;

        } catch (error) {
            console.error('Error loading accounts:', error);
        }
    }

    findDepositBtn.addEventListener('click', async function () {
        const depositNumber = depositNumberInput.value.trim();

        if (!depositNumber) {
            document.getElementById('depositNumberError').style.display = 'block';
            return;
        }

        document.getElementById('depositNumberError').style.display = 'none';
        loading.style.display = 'block';

        try {
            setTimeout(() => {

                const depositData = {
                    id: 'dep-' + depositNumber,
                    depositNumber: depositNumber,
                    depositType: 'Накопительный',
                    amount: 100000,
                    interestRate: 6.5,
                    termMonths: 12,
                    startDate: '2024-01-15',
                    endDate: '2025-01-15',
                    isBlocked: false,
                    isClosed: false,
                    clientId: 'client-123'
                };

                currentDeposit = depositData;

                displayDepositInfo(depositData);

                loadClientAccounts(depositData.clientId);

                const closableCheck = checkDepositClosable(depositData);
                displayRequirements(closableCheck.requirements);

                const today = new Date();
                const endDate = new Date(depositData.endDate);
                const isEarly = today < endDate;
                const calculation = calculateClosingAmount(depositData, isEarly);
                displayCalculations(calculation);

                loading.style.display = 'none';
            }, 1000);

        } catch (error) {
            console.error('Error:', error);
            loading.style.display = 'none';
            alert('Ошибка при поиске вклада: ' + error.message);
        }
    });

    confirmationCheckbox.addEventListener('change', function () {
        closeDepositBtn.disabled = !this.checked;
    });

    closeDepositBtn.addEventListener('click', async function () {
        const reason = document.getElementById('closingReason').value;
        const destinationAccount = destinationAccountSelect.value;

        if (!reason) {
            document.getElementById('reasonError').style.display = 'block';
            return;
        }

        if (!destinationAccount) {
            document.getElementById('accountError').style.display = 'block';
            return;
        }

        if (!confirmationCheckbox.checked) {
            document.getElementById('confirmationError').style.display = 'block';
            return;
        }

        document.getElementById('reasonError').style.display = 'none';
        document.getElementById('accountError').style.display = 'none';
        document.getElementById('confirmationError').style.display = 'none';

        loading.style.display = 'block';

        try {
            setTimeout(() => {

                loading.style.display = 'none';
                closeDepositSection.style.display = 'none';
                successMessage.style.display = 'block';

                const today = new Date();
                const endDate = new Date(currentDeposit.endDate);
                const isEarly = today < endDate;
                const calculation = calculateClosingAmount(currentDeposit, isEarly);

                successMessage.innerHTML = `
                            Вклад успешно закрыт!<br>
                            Номер вклада: <strong>${currentDeposit.depositNumber}</strong><br>
                            Сумма к зачислению: <strong>${calculation.totalAmount.toLocaleString('ru-RU')} руб.</strong><br>
                            Счет зачисления: <strong>${destinationAccountSelect.options[destinationAccountSelect.selectedIndex].text}</strong><br>
                            Дата закрытия: <strong>${new Date().toLocaleDateString('ru-RU')}</strong>
                        `;

            }, 2000);

        } catch (error) {
            console.error('Error:', error);
            loading.style.display = 'none';
            alert('Ошибка при закрытии вклада: ' + error.message);
        }
    });
});