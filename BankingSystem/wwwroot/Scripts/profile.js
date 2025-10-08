document.addEventListener('DOMContentLoaded', function () {
    const findClientBtn = document.getElementById('findClientBtn');
    const clientInfo = document.getElementById('clientInfo');
    const clientDetails = document.getElementById('clientDetails');
    const accountsSection = document.getElementById('accountsSection');
    const creditsSection = document.getElementById('creditsSection');
    const depositsSection = document.getElementById('depositsSection');
    const accountsList = document.getElementById('accountsList');
    const creditsList = document.getElementById('creditsList');
    const depositsList = document.getElementById('depositsList');
    const loading = document.getElementById('loading');

    const passportSeriesInput = document.getElementById('passportSeries');
    const passportNumberInput = document.getElementById('passportNumber');

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

    function getCookie(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) return parts.pop().split(';').shift();
        return null;
    }

    async function makeAuthenticatedRequest(url) {
        const token = getCookie('jwt');

        if (!token) {
            throw new Error('JWT token not found');
        }

        const response = await fetch(url, {
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        return await response.json();
    }

    function displayClientData(client) {
        clientDetails.innerHTML = `
                    <div class="client-detail">
                        <strong>ID клиента:</strong>
                        <span>${client.id}</span>
                    </div>
                    <div class="client-detail">
                        <strong>ФИО:</strong>
                        <span>${client.lastName} ${client.firstName} ${client.secondName}</span>
                    </div>
                    <div class="client-detail">
                        <strong>Дата рождения:</strong>
                        <span>${new Date(client.birthDate).toLocaleDateString('ru-RU')}</span>
                    </div>
                    <div class="client-detail">
                        <strong>Паспорт:</strong>
                        <span>${client.passportSeries} ${client.passportNumber}</span>
                    </div>
                    <div class="client-detail">
                        <strong>Телефон:</strong>
                        <span>${client.phoneNumber}</span>
                    </div>
                    <div class="client-detail">
                        <strong>Email:</strong>
                        <span>${client.emailAddress}</span>
                    </div>
                `;

        clientInfo.style.display = 'block';
    }

    function displayAccounts(accounts) {
        if (accounts.length === 0) {
            accountsList.innerHTML = '<div class="no-data">Счета не найдены</div>';
            return;
        }

        accountsList.innerHTML = accounts.map(account => `
                    <div class="card">
                        <div class="card-header">
                            <div class="card-title">Счет №${account.accountNumber}</div>
                            <div class="card-badge">${account.accountType}</div>
                        </div>
                        <div class="card-content">
                            <div class="card-item">
                                <div class="card-label">Баланс</div>
                                <div class="card-value ${account.balance >= 0 ? 'positive' : 'negative'}">
                                    ${account.balance.toLocaleString('ru-RU')} ${account.currencyCode}
                                </div>
                            </div>
                            <div class="card-item">
                                <div class="card-label">Валюта</div>
                                <div class="card-value">${account.currencyCode}</div>
                            </div>
                            <div class="card-item">
                                <div class="card-label">Тип счета</div>
                                <div class="card-value">${account.accountType}</div>
                            </div>
                            <div class="card-item">
                                <div class="card-label">Статус</div>
                                <div class="card-value">${account.isActive ? 'Активный' : 'Закрыт'}</div>
                            </div>
                        </div>
                    </div>
                `).join('');

        accountsSection.style.display = 'block';
    }

    function displayCredits(credits) {
        if (credits.length === 0) {
            creditsList.innerHTML = '<div class="no-data">Кредиты не найдены</div>';
            return;
        }

        creditsList.innerHTML = credits.map(credit => `
                    <div class="card">
                        <div class="card-header">
                            <div class="card-title">Кредит №${credit.accountId}</div>
                            <div class="card-badge">${credit.paymentMonth}</div>
                        </div>
                        <div class="card-content">
                            <div class="card-item">
                                <div class="card-label">Сумма кредита</div>
                                <div class="card-value">${credit.sumCredit.toLocaleString('ru-RU')} руб.</div>
                            </div>
                            <div class="card-item">
                                <div class="card-label">Остаток долга</div>
                                <div class="card-value">${credit.LeftCredit.toLocaleString('ru-RU')} руб.</div>
                            </div>
                            <div class="card-item">
                                <div class="card-label">Срок</div>
                                <div class="card-value">${credit.termMonths} мес.</div>
                            </div>
                            <div class="card-item">
                                <div class="card-label">Ежемесячный платеж</div>
                                <div class="card-value">${credit.paymentMonth.toLocaleString('ru-RU')} руб.</div>
                            </div>
                            <div class="card-item">
                                <div class="card-label">Дата открытия</div>
                                <div class="card-value">${new Date(credit.startDate).toLocaleDateString('ru-RU')}</div>
                            </div>
                        </div>
                    </div>
                `).join('');

        creditsSection.style.display = 'block';
    }

    function displayDeposits(deposits) {
        if (deposits.length === 0) {
            depositsList.innerHTML = '<div class="no-data">Вклады не найдены</div>';
            return;
        }

        depositsList.innerHTML = deposits.map(deposit => `
                    <div class="card">
                        <div class="card-header">
                            <div class="card-title">Вклад №${deposit.id}</div>
                            <div class="card-badge">${deposit.depositType}</div>
                        </div>
                        <div class="card-content">
                            <div class="card-item">
                                <div class="card-label">Сумма вклада</div>
                                <div class="card-value positive">${deposit.sumDeposit.toLocaleString('ru-RU')} руб.</div>
                            </div>
                            <div class="card-item">
                                <div class="card-label">Процентная ставка</div>
                                <div class="card-value">${deposit.percentYear}%</div>
                            </div>
                            <div class="card-item">
                                <div class="card-label">Срок</div>
                                <div class="card-value">${deposit.termMonth} мес.</div>
                            </div>
                            <div class="card-item">
                                <div class="card-label">Дата открытия</div>
                                <div class="card-value">${new Date(deposit.startDate).toLocaleDateString('ru-RU')}</div>
                            </div>
                            <div class="card-item">
                                <div class="card-label">Дата закрытия</div>
                                <div class="card-value">${deposit.endDate ? new Date(deposit.endDate).toLocaleDateString('ru-RU') : 'Не определено'}</div>
                            </div>
                        </div>
                    </div>
                `).join('');

        depositsSection.style.display = 'block';
    }

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

        loading.style.display = 'block';
        clientInfo.style.display = 'none';
        accountsSection.style.display = 'none';
        creditsSection.style.display = 'none';
        depositsSection.style.display = 'none';

        const response = await fetch('/getProfile', {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                passportSeries: series,
                passportNumber: number
            })
        });

        if (!response.ok) {
            alert('Клиента не найден');
        }

        const responseData = await response.json();

        displayClientData(responseData.client);
        displayAccounts(response.accounts);
        displayCredits(response.credits);
        displayDeposits(response.deposits);


        //try {

        //    setTimeout(async () => {
        //        try {

        //            const clientData = {
        //                id: 'a1b2c3d4-e5f6-7890-abcd-ef1234567890',
        //                firstName: 'Иван',
        //                lastName: 'Петров',
        //                secondName: 'Сергеевич',
        //                birthDate: '1985-05-15',
        //                passportSeries: series,
        //                passportNumber: number,
        //                phoneNumber: '+7 (912) 345-67-89',
        //                emailAddress: 'ivan.petrov@example.com',
        //                addressRegistration: 'г. Москва, ул. Примерная, д. 123, кв. 45'
        //            };

        //            const accountsData = [
        //                {
        //                    id: 'acc-001',
        //                    accountNumber: '40817810000000000001',
        //                    accountType: 'Расчетный',
        //                    balance: 15000,
        //                    currencyCode: 'RUB',
        //                    isActive: true
        //                },
        //                {
        //                    id: 'acc-002',
        //                    accountNumber: '40817810000000000002',
        //                    accountType: 'Сберегательный',
        //                    balance: 50000,
        //                    currencyCode: 'RUB',
        //                    isActive: true
        //                }
        //            ];

        //            const creditsData = [
        //                {
        //                    id: 'credit-001',
        //                    creditNumber: 'CR20240001',
        //                    amount: 100000,
        //                    remainingAmount: 75000,
        //                    interestRate: 12.5,
        //                    termMonths: 24,
        //                    monthlyPayment: 4583.33,
        //                    startDate: '2024-01-15',
        //                    status: 'Активный'
        //                }
        //            ];

        //            const depositsData = [
        //                {
        //                    id: 'deposit-001',
        //                    depositNumber: 'DP20240001',
        //                    depositType: 'Накопительный',
        //                    amount: 50000,
        //                    interestRate: 6.5,
        //                    termMonths: 12,
        //                    earnedInterest: 1625.00,
        //                    startDate: '2024-03-01',
        //                    endDate: '2025-03-01'
        //                }
        //            ];

        //            displayClientData(clientData);
        //            displayAccounts(accountsData);
        //            displayCredits(creditsData);
        //            displayDeposits(depositsData);

        //        } catch (error) {
        //            console.error('Error loading data:', error);
        //            alert('Ошибка при загрузке данных: ' + error.message);
        //        } finally {
        //            loading.style.display = 'none';
        //        }
        //    }, 1000);

        //} catch (error) {
        //    console.error('Error:', error);
        //    loading.style.display = 'none';
        //    alert('Ошибка: ' + error.message);
        //}
    });
});