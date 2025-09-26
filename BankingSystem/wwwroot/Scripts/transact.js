document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('transferForm');
    const successMessage = document.getElementById('successMessage');
    const senderAccountSelect = document.getElementById('senderAccount');
    const senderAccountInfo = document.getElementById('senderAccountInfo');
    const receiverAccountInput = document.getElementById('receiverAccount');
    const receiverAccountInfo = document.getElementById('receiverAccountInfo');
    const transferAmountInput = document.getElementById('transferAmount');

    const accountsDatabase = [
        {
            id: 'acc-001',
            number: '4081 7810 0000 0000 0001',
            type: 'Расчетный',
            balance: 15000,
            currency: 'RUB',
            owner: 'Иван Петров Сергеевич'
        },
        {
            id: 'acc-002',
            number: '4081 7810 0000 0000 0002',
            type: 'Сберегательный',
            balance: 50000,
            currency: 'RUB',
            owner: 'Иван Петров Сергеевич'
        },
        {
            id: 'acc-003',
            number: '4081 7810 0000 0000 0003',
            type: 'Расчетный',
            balance: 25000,
            currency: 'RUB',
            owner: 'Мария Иванова Александровна'
        },
        {
            id: 'acc-004',
            number: '4081 7810 0000 0000 0004',
            type: 'Расчетный',
            balance: 100000,
            currency: 'RUB',
            owner: 'Алексей Сидоров Владимирович'
        }
    ];

    senderAccountSelect.addEventListener('change', function () {
        const accountId = this.value;

        if (accountId) {
            const account = accountsDatabase.find(acc => acc.id === accountId);

            if (account) {
                document.getElementById('senderAccountNumber').textContent = account.number;
                document.getElementById('senderAccountType').textContent = account.type;
                document.getElementById('senderAccountBalance').textContent = account.balance.toLocaleString('ru-RU') + ' ' + account.currency;
                document.getElementById('senderAccountCurrency').textContent = account.currency;

                senderAccountInfo.style.display = 'block';

                transferAmountInput.max = account.balance;
            }
        } else {
            senderAccountInfo.style.display = 'none';
        }
    });

    receiverAccountInput.addEventListener('blur', function () {
        const accountNumber = this.value.replace(/\s/g, '');

        if (accountNumber.length === 20 && /^\d+$/.test(accountNumber)) {
            this.value = accountNumber.replace(/(\d{4})(\d{4})(\d{4})(\d{4})(\d{4})/, '$1 $2 $3 $4 $5');

            const foundAccount = accountsDatabase.find(acc =>
                acc.number.replace(/\s/g, '') === accountNumber
            );

            if (foundAccount) {
                document.getElementById('receiverAccountNumber').textContent = foundAccount.number;
                document.getElementById('receiverAccountOwner').textContent = foundAccount.owner;
                document.getElementById('receiverAccountType').textContent = foundAccount.type;
                document.getElementById('receiverAccountCurrency').textContent = foundAccount.currency;

                receiverAccountInfo.style.display = 'block';
                document.getElementById('receiverAccountError').style.display = 'none';
            } else {
                document.getElementById('receiverAccountNumber').textContent = this.value;
                document.getElementById('receiverAccountOwner').textContent = 'Внешний клиент';
                document.getElementById('receiverAccountType').textContent = 'Неизвестно';
                document.getElementById('receiverAccountCurrency').textContent = 'RUB';

                receiverAccountInfo.style.display = 'block';
                document.getElementById('receiverAccountError').style.display = 'none';
            }
        } else if (accountNumber.length > 0) {
            document.getElementById('receiverAccountError').style.display = 'block';
            receiverAccountInfo.style.display = 'none';
        } else {
            document.getElementById('receiverAccountError').style.display = 'none';
            receiverAccountInfo.style.display = 'none';
        }
    });

    receiverAccountInput.addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');

        if (value.length > 20) {
            value = value.substring(0, 20);
        }

        if (value.length > 0) {
            value = value.replace(/(\d{4})/g, '$1 ').trim();
        }

        e.target.value = value;
    });

    transferAmountInput.addEventListener('input', function () {
        const amount = parseFloat(this.value) || 0;
        const senderAccountId = senderAccountSelect.value;

        if (senderAccountId) {
            const senderAccount = accountsDatabase.find(acc => acc.id === senderAccountId);

            if (senderAccount && amount > senderAccount.balance) {
                document.getElementById('amountError').textContent = 'Недостаточно средств на счете';
                document.getElementById('amountError').style.display = 'block';
            } else if (amount <= 0) {
                document.getElementById('amountError').textContent = 'Введите корректную сумму';
                document.getElementById('amountError').style.display = 'block';
            } else {
                document.getElementById('amountError').style.display = 'none';
            }
        }
    });

    form.addEventListener('submit', function (event) {
        event.preventDefault();
        let isValid = true;

        if (!senderAccountSelect.value) {
            document.getElementById('senderAccountError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('senderAccountError').style.display = 'none';
        }

        const receiverAccount = receiverAccountInput.value.replace(/\s/g, '');
        if (receiverAccount.length !== 20 || !/^\d+$/.test(receiverAccount)) {
            document.getElementById('receiverAccountError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('receiverAccountError').style.display = 'none';
        }

        const amount = parseFloat(transferAmountInput.value) || 0;
        const senderAccount = accountsDatabase.find(acc => acc.id === senderAccountSelect.value);

        if (!amount || amount <= 0) {
            document.getElementById('amountError').textContent = 'Введите корректную сумму';
            document.getElementById('amountError').style.display = 'block';
            isValid = false;
        } else if (senderAccount && amount > senderAccount.balance) {
            document.getElementById('amountError').textContent = 'Недостаточно средств на счете';
            document.getElementById('amountError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('amountError').style.display = 'none';
        }

        if (isValid) {
            successMessage.innerHTML = `
                        Перевод успешно выполнен!<br>
                        Сумма: <strong>${amount.toLocaleString('ru-RU')} руб.</strong><br>
                        Со счета: <strong>${document.getElementById('senderAccountNumber').textContent}</strong><br>
                        На счет: <strong>${receiverAccountInput.value}</strong>
                    `;

            successMessage.style.display = 'block';
            form.reset();
            senderAccountInfo.style.display = 'none';
            receiverAccountInfo.style.display = 'none';

            successMessage.scrollIntoView({ behavior: 'smooth' });

            setTimeout(function () {
                successMessage.style.display = 'none';
            }, 5000);
        }
    });
});