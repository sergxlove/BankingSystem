document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('accountForm');
    const findClientBtn = document.getElementById('findClientBtn');
    const clientInfo = document.getElementById('clientInfo');
    const accountDetails = document.getElementById('accountDetails');
    const successMessage = document.getElementById('successMessage');

    const passportSeriesInput = document.getElementById('passportSeries');
    const passportNumberInput = document.getElementById('passportNumber');
    const clientIdSpan = document.getElementById('clientId');
    const clientNameSpan = document.getElementById('clientName');
    const clientBirthDateSpan = document.getElementById('clientBirthDate');

    const accountTypeSelect = document.getElementById('accountType');
    const currencyCodeSelect = document.getElementById('currencyCode');
    const initialBalanceInput = document.getElementById('initialBalance');

    passportSeriesInput.addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/\D/g, '').substring(0, 4);
    });

    passportNumberInput.addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/\D/g, '').substring(0, 6);
    });

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
            passportNumber: '567890'
        },
        {
            id: 'b2c3d4e5-f6g7-8901-bcde-f23456789012',
            firstName: 'Мария',
            lastName: 'Иванова',
            secondName: 'Александровна',
            birthDate: '1990-12-22',
            passportSeries: '4321',
            passportNumber: '098765'
        },
        {
            id: 'c3d4e5f6-g7h8-9012-cdef-345678901234',
            firstName: 'Алексей',
            lastName: 'Сидоров',
            secondName: 'Владимирович',
            birthDate: '1978-08-03',
            passportSeries: '1111',
            passportNumber: '222222'
        }
    ];

    findClientBtn.addEventListener('click', async function () {
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

        const response = await fetch('/getShortClient', {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                passportSeries: series,
                passportNumber: number
            })
        });
        if (response.ok) {
            const responseData = await response.json();
            clientIdSpan.textContent = responseData.id;
            clientNameSpan.textContent = responseData.name;

            clientBirthDateSpan.textContent = responseData.dateBirth;

            clientInfo.style.display = 'block';
            accountDetails.style.display = 'block';

            accountDetails.scrollIntoView({ behavior: 'smooth' });
        }
        else {
            alert(await response.text());
        }
    });

    function generateAccountNumber() {
        const prefix = '408';
        const random = Math.floor(1000000000000 + Math.random() * 9000000000000);
        return prefix + random.toString().substring(0, 17);
    }

    form.addEventListener('submit', async function (event) {
        event.preventDefault();
        let isValid = true;

        if (clientIdSpan.textContent === '') {
            alert('Сначала найдите клиента по паспортным данным!');
            return;
        }

        if (!accountTypeSelect.value) {
            document.getElementById('accountTypeError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('accountTypeError').style.display = 'none';
        }

        if (!currencyCodeSelect.value) {
            document.getElementById('currencyError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('currencyError').style.display = 'none';
        }

        if (!initialBalanceInput.value || parseFloat(initialBalanceInput.value) < 0) {
            document.getElementById('balanceError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('balanceError').style.display = 'none';
        }

        if (isValid) {
            const accountNumber = generateAccountNumber();

            const response = await fetch('/regAccount', {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    ClientsId: clientIdSpan.textContent,
                    AccountType: accountTypeSelect.options[accountTypeSelect.selectedIndex].text,
                    Balance: parseFloat(initialBalanceInput.value).toFixed(2),
                    CurrencyCode: currencyCodeSelect.value
                })
            });

            if (response.ok) {
                const successAlert =
                    "✅ СЧЕТ УСПЕШНО ОТКРЫТ!\n\n" +
                    `📋 Номер счета: ${accountNumber}\n` +
                    `📊 Тип счета: ${accountTypeSelect.options[accountTypeSelect.selectedIndex].text}\n` +
                    `💰 Валюта: ${currencyCodeSelect.value}\n` +
                    `💳 Начальный баланс: ${parseFloat(initialBalanceInput.value).toFixed(2)}`;

                alert(successAlert);
                form.reset();
                clientInfo.style.display = 'none';
                accountDetails.style.display = 'none';
            }
            else {
                alert(await response.text);
            }

        }
    });
});