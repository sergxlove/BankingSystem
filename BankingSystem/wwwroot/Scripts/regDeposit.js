﻿document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('depositForm');
    const findClientBtn = document.getElementById('findClientBtn');
    const clientInfo = document.getElementById('clientInfo');
    const depositDetails = document.getElementById('depositDetails');
    const successMessage = document.getElementById('successMessage');
    const accountsList = document.getElementById('accountsList');
    const depositIncomeElement = document.getElementById('depositIncome');
    const totalAmountElement = document.getElementById('totalAmount');
    const interestRateElement = document.getElementById('interestRate');
    const termValueElement = document.getElementById('termValue');

    const passportSeriesInput = document.getElementById('passportSeries');
    const passportNumberInput = document.getElementById('passportNumber');
    const clientIdSpan = document.getElementById('clientId');
    const clientNameSpan = document.getElementById('clientName');
    const clientBirthDateSpan = document.getElementById('clientBirthDate');

    const depositAmountInput = document.getElementById('depositAmount');
    const depositTermInput = document.getElementById('depositTerm');

    let selectedAccountId = null;
    let currentClientId = null;

    passportSeriesInput.addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/\D/g, '').substring(0, 4);
    });

    passportNumberInput.addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/\D/g, '').substring(0, 6);
    });

    depositTermInput.addEventListener('input', function () {
        termValueElement.textContent = this.value;
        updateInterestRate();
        calculateDeposit();
    });

    depositAmountInput.addEventListener('input', calculateDeposit);

    function updateInterestRate() {
        const term = parseInt(depositTermInput.value);
        let rate = 5.0;

        if (term >= 24) {
            rate = 6.5;
        } else if (term >= 12) {
            rate = 6.0;
        } else if (term >= 6) {
            rate = 5.5;
        }

        interestRateElement.textContent = rate + '%';
    }

    function calculateDeposit() {
        const amount = parseFloat(depositAmountInput.value) || 0;
        const term = parseInt(depositTermInput.value);
        const rate = parseFloat(interestRateElement.textContent) / 100;

        if (amount > 0 && term > 0) {
            const income = amount * rate * (term / 12);
            const total = amount + income;

            depositIncomeElement.textContent = income.toFixed(2) + ' руб.';
            totalAmountElement.textContent = total.toFixed(2) + ' руб.';
        } else {
            depositIncomeElement.textContent = '—';
            totalAmountElement.textContent = '—';
        }
    }

    function validatePassportSeries(series) {
        return /^\d{4}$/.test(series);
    }

    function validatePassportNumber(number) {
        return /^\d{6}$/.test(number);
    }

    const clientsDatabase = [
        {
            id: 'a1b2c3d4-e5f6-7890-abcd-ef1234567890',
            firstName: 'Иван',
            lastName: 'Петров',
            secondName: 'Сергеевич',
            birthDate: '1985-05-15',
            passportSeries: '1234',
            passportNumber: '567890',
            accounts: [
                { id: 'acc-001', number: '40817810000000000001', balance: 15000, type: 'Расчетный', currency: 'RUB' },
                { id: 'acc-002', number: '40817810000000000002', balance: 50000, type: 'Сберегательный', currency: 'RUB' }
            ]
        },
        {
            id: 'b2c3d4e5-f6g7-8901-bcde-f23456789012',
            firstName: 'Мария',
            lastName: 'Иванова',
            secondName: 'Александровна',
            birthDate: '1990-12-22',
            passportSeries: '4321',
            passportNumber: '098765',
            accounts: [
                { id: 'acc-003', number: '40817810000000000003', balance: 25000, type: 'Расчетный', currency: 'RUB' }
            ]
        },
        {
            id: 'c3d4e5f6-g7h8-9012-cdef-345678901234',
            firstName: 'Алексей',
            lastName: 'Сидоров',
            secondName: 'Владимирович',
            birthDate: '1978-08-03',
            passportSeries: '1111',
            passportNumber: '222222',
            accounts: [
                { id: 'acc-004', number: '40817810000000000004', balance: 100000, type: 'Расчетный', currency: 'RUB' },
                { id: 'acc-005', number: '40817810000000000005', balance: 5000, type: 'Сберегательный', currency: 'USD' }
            ]
        }
    ];

    function displayAccounts(accounts) {
        accountsList.innerHTML = '';

        if (accounts.length === 0) {
            accountsList.innerHTML = '<p>У клиента нет открытых счетов</p>';
            return;
        }

        const rubAccounts = accounts.filter(acc => acc.currency === 'RUB');

        if (rubAccounts.length === 0) {
            accountsList.innerHTML = '<p>У клиента нет рублевых счетов для открытия вклада</p>';
            return;
        }

        rubAccounts.forEach(account => {
            const accountElement = document.createElement('div');
            accountElement.className = 'account-card';
            accountElement.innerHTML = `
                        <div class="account-number">${account.number}</div>
                        <div class="account-type">${account.type} • ${account.currency}</div>
                        <div class="account-balance">Баланс: ${account.balance.toLocaleString('ru-RU')} ${account.currency}</div>
                    `;

            accountElement.addEventListener('click', function () {
                document.querySelectorAll('.account-card').forEach(card => {
                    card.classList.remove('selected');
                });

                this.classList.add('selected');
                selectedAccountId = account.id;

                depositAmountInput.max = account.balance;
            });

            accountsList.appendChild(accountElement);
        });
    }

    findClientBtn.addEventListener('click', function () {
        const series = passportSeriesInput.value;
        const number = passportNumberInput.value;

        if (!validatePassportSeries(series)) {
            document.getElementById('passportSeriesError').style.display = 'block';
            return;
        } else {
            document.getElementById('passportSeriesError').style.display = 'none';
        }

        if (!validatePassportNumber(number)) {
            document.getElementById('passportNumberError').style.display = 'block';
            return;
        } else {
            document.getElementById('passportNumberError').style.display = 'none';
        }

        const client = clientsDatabase.find(c =>
            c.passportSeries === series && c.passportNumber === number
        );

        if (client) {
            currentClientId = client.id;
            clientIdSpan.textContent = client.id;
            clientNameSpan.textContent = `${client.lastName} ${client.firstName} ${client.secondName}`;

            const birthDate = new Date(client.birthDate);
            const formattedDate = birthDate.toLocaleDateString('ru-RU');
            clientBirthDateSpan.textContent = formattedDate;

            displayAccounts(client.accounts);

            updateInterestRate();
            calculateDeposit();

            clientInfo.style.display = 'block';
            depositDetails.style.display = 'block';

            depositDetails.scrollIntoView({ behavior: 'smooth' });
        } else {
            alert('Клиент с указанными паспортными данными не найден!');
        }
    });

    form.addEventListener('submit', function (event) {
        event.preventDefault();
        let isValid = true;
        if (!currentClientId) {
            alert('Сначала найдите клиента по паспортным данным!');
            return;
        }

        if (!selectedAccountId) {
            document.getElementById('accountError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('accountError').style.display = 'none';
        }

        if (!depositAmountInput.value || parseFloat(depositAmountInput.value) < 1000) {
            document.getElementById('amountError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('amountError').style.display = 'none';
        }

        if (isValid) {
            const amount = parseFloat(depositAmountInput.value);
            const term = parseInt(depositTermInput.value);
            const rate = parseFloat(interestRateElement.textContent);
            const income = amount * (rate / 100) * (term / 12);
            const total = amount + income;

            successMessage.innerHTML = `
                        Вклад успешно оформлен!<br>
                        ID клиента: <strong>${currentClientId}</strong><br>
                        ID счета: <strong>${selectedAccountId}</strong><br>
                        Сумма вклада: <strong>${amount.toLocaleString('ru-RU')} руб.</strong><br>
                        Срок: <strong>${term} месяцев</strong><br>
                        Процентная ставка: <strong>${rate}% годовых</strong><br>
                        Ожидаемый доход: <strong>${income.toFixed(2)} руб.</strong>
                    `;

            successMessage.style.display = 'block';
            form.reset();
            clientInfo.style.display = 'none';
            depositDetails.style.display = 'none';
            selectedAccountId = null;
            currentClientId = null;

            depositIncomeElement.textContent = '—';
            totalAmountElement.textContent = '—';
            termValueElement.textContent = '12';

            successMessage.scrollIntoView({ behavior: 'smooth' });

            setTimeout(function () {
                successMessage.style.display = 'none';
            }, 5000);
        }
    });
});