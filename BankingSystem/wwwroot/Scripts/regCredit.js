document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('creditForm');
    const findClientBtn = document.getElementById('findClientBtn');
    const clientInfo = document.getElementById('clientInfo');
    const creditDetails = document.getElementById('creditDetails');
    const successMessage = document.getElementById('successMessage');
    const accountsList = document.getElementById('accountsList');
    const monthlyPaymentElement = document.getElementById('monthlyPayment');
    const totalPaymentElement = document.getElementById('totalPayment');
    const termValueElement = document.getElementById('termValue');

    const passportSeriesInput = document.getElementById('passportSeries');
    const passportNumberInput = document.getElementById('passportNumber');
    const clientIdSpan = document.getElementById('clientId');
    const clientNameSpan = document.getElementById('clientName');
    const clientBirthDateSpan = document.getElementById('clientBirthDate');

    const creditAmountInput = document.getElementById('creditAmount');
    const creditTermInput = document.getElementById('creditTerm');

    let selectedAccountId = null;
    let currentClientId = null;

    passportSeriesInput.addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/\D/g, '').substring(0, 4);
    });

    passportNumberInput.addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/\D/g, '').substring(0, 6);
    });

    creditTermInput.addEventListener('input', function () {
        termValueElement.textContent = this.value;
        calculatePayments();
    });

    creditAmountInput.addEventListener('input', calculatePayments);

    function calculatePayments() {
        const amount = parseFloat(creditAmountInput.value) || 0;
        const term = parseInt(creditTermInput.value);

        if (amount > 0 && term > 0) {
            const monthlyRate = 0.12 / 12;
            const annuityRatio = monthlyRate * Math.pow(1 + monthlyRate, term) / (Math.pow(1 + monthlyRate, term) - 1);
            const monthlyPayment = amount * annuityRatio;
            const totalPayment = monthlyPayment * term;

            monthlyPaymentElement.textContent = monthlyPayment.toFixed(2) + ' руб.';
            totalPaymentElement.textContent = totalPayment.toFixed(2) + ' руб.';
        } else {
            monthlyPaymentElement.textContent = '—';
            totalPaymentElement.textContent = '—';
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

        accounts.forEach(account => {
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

            clientInfo.style.display = 'block';
            creditDetails.style.display = 'block';

            creditDetails.scrollIntoView({ behavior: 'smooth' });
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

        if (!creditAmountInput.value || parseFloat(creditAmountInput.value) < 1000) {
            document.getElementById('amountError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('amountError').style.display = 'none';
        }

        if (isValid) {
            successMessage.innerHTML = `
                        Кредит успешно оформлен!<br>
                        ID клиента: <strong>${currentClientId}</strong><br>
                        ID счета: <strong>${selectedAccountId}</strong><br>
                        Сумма кредита: <strong>${parseFloat(creditAmountInput.value).toLocaleString('ru-RU')} руб.</strong><br>
                        Срок: <strong>${creditTermInput.value} месяцев</strong>
                    `;

            successMessage.style.display = 'block';
            form.reset();
            clientInfo.style.display = 'none';
            creditDetails.style.display = 'none';
            selectedAccountId = null;
            currentClientId = null;

            monthlyPaymentElement.textContent = '—';
            totalPaymentElement.textContent = '—';
            termValueElement.textContent = '12';

            successMessage.scrollIntoView({ behavior: 'smooth' });

            setTimeout(function () {
                successMessage.style.display = 'none';
            }, 5000);
        }
    });
});