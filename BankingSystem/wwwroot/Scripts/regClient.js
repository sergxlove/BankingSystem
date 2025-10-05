document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('registrationForm');
    const successMessage = document.getElementById('successMessage');

    const firstNameInput = document.getElementById('firstName');
    const secondNameInput = document.getElementById('secondName');
    const lastNameInput = document.getElementById('lastName');
    const birthDateInput = document.getElementById('birthDate');
    const passportSeriesInput = document.getElementById('passportSeries');
    const passportNumberInput = document.getElementById('passportNumber');
    const phoneNumberInput = document.getElementById('phoneNumber');
    const emailInput = document.getElementById('emailAddress');
    const addressInput = document.getElementById('addressRegistration');

    function validateName(name) {
        return name.length >= 2 && /^[a-zA-Zа-яА-ЯёЁ\s\-]+$/.test(name);
    }

    function validateBirthDate(date) {
        const birthDate = new Date(date);
        const today = new Date();
        const minDate = new Date();
        minDate.setFullYear(today.getFullYear() - 120);

        return birthDate <= today && birthDate >= minDate;
    }

    function validatePassportSeries(series) {
        return /^\d{4}$/.test(series);
    }

    function validatePassportNumber(number) {
        return /^\d{6}$/.test(number);
    }

    function validatePhone(phone) {
        return /^(\+7|8)[\s\-]?\(?\d{3}\)?[\s\-]?\d{3}[\s\-]?\d{2}[\s\-]?\d{2}$/.test(phone);
    }

    function validateEmail(email) {
        return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email);
    }

    function validateAddress(address) {
        return address.length >= 10;
    }

    phoneNumberInput.addEventListener('input', function (e) {
        let value = e.target.value.replace(/\D/g, '');

        if (value.startsWith('7') || value.startsWith('8')) {
            value = value.substring(1);
        }

        if (value.length > 0) {
            value = '+7 (' + value;

            if (value.length > 7) {
                value = value.substring(0, 7) + ') ' + value.substring(7);
            }
            if (value.length > 12) {
                value = value.substring(0, 12) + '-' + value.substring(12);
            }
            if (value.length > 15) {
                value = value.substring(0, 15) + '-' + value.substring(15);
            }
        }

        e.target.value = value;
    });

    passportSeriesInput.addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/\D/g, '').substring(0, 4);
    });

    passportNumberInput.addEventListener('input', function (e) {
        e.target.value = e.target.value.replace(/\D/g, '').substring(0, 6);
    });

    form.addEventListener('submit', function (event) {
        event.preventDefault();
        
        try {
            const response = await fetch("/regClient", {
                method: "POST",
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    FirstName: firstNameInput,
                    SecondName: secondNameInput,
                    LastName: lastNameInput,
                    BirthDate: birthDateInput,
                    PassportSeries: passportSeriesInput,
                    PassportNumber: passportNumberInput,
                    PhoneNumber: phoneNumberInput,
                    EmailAddres: emailInput,
                    AddressRegistration: addressInput
                })
            });

            switch (response.status) {
                case 200:
                    alert('Клиент успешно доавлен');
                    break;
                case 400:
                    alert(await response.text())
                    break;
                default:
                    alert('Произошла ошибка');
                    break;
            }
        }
        catch (error) {
            alert('Ошибка соединения с сервером');
        }
    });
});