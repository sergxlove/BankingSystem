document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('registrationForm');
    const successMessage = document.getElementById('successMessage');

    const firstNameInput = document.getElementById('firstName');
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
        let isValid = true;

        if (!validateName(firstNameInput.value)) {
            document.getElementById('firstNameError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('firstNameError').style.display = 'none';
        }

        if (!validateName(lastNameInput.value)) {
            document.getElementById('lastNameError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('lastNameError').style.display = 'none';
        }

        if (!validateBirthDate(birthDateInput.value)) {
            document.getElementById('birthDateError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('birthDateError').style.display = 'none';
        }

        if (!validatePassportSeries(passportSeriesInput.value)) {
            document.getElementById('passportSeriesError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('passportSeriesError').style.display = 'none';
        }

        if (!validatePassportNumber(passportNumberInput.value)) {
            document.getElementById('passportNumberError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('passportNumberError').style.display = 'none';
        }

        if (!validatePhone(phoneNumberInput.value)) {
            document.getElementById('phoneNumberError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('phoneNumberError').style.display = 'none';
        }

        if (!validateEmail(emailInput.value)) {
            document.getElementById('emailError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('emailError').style.display = 'none';
        }

        if (!validateAddress(addressInput.value)) {
            document.getElementById('addressError').style.display = 'block';
            isValid = false;
        } else {
            document.getElementById('addressError').style.display = 'none';
        }

        if (isValid) {
            successMessage.style.display = 'block';
            form.reset();

            setTimeout(function () {
                successMessage.style.display = 'none';
            }, 3000);
        }
    });
});