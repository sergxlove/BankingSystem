document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('registrationForm');
    const usernameInput = document.getElementById('username');
    const passwordInput = document.getElementById('password');
    const confirmPasswordInput = document.getElementById('confirmPassword');
    const usernameError = document.getElementById('usernameError');
    const passwordError = document.getElementById('passwordError');
    const confirmPasswordError = document.getElementById('confirmPasswordError');
    const successMessage = document.getElementById('successMessage');
    const togglePassword = document.getElementById('togglePassword');
    const toggleConfirmPassword = document.getElementById('toggleConfirmPassword');

    function setupPasswordToggle(toggleElement, inputElement) {
        toggleElement.addEventListener('click', function () {
            if (inputElement.type === 'password') {
                inputElement.type = 'text';
                toggleElement.textContent = 'Скрыть';
            } else {
                inputElement.type = 'password';
                toggleElement.textContent = 'Показать';
            }
        });
    }

    setupPasswordToggle(togglePassword, passwordInput);
    setupPasswordToggle(toggleConfirmPassword, confirmPasswordInput);

    form.addEventListener('submit', function (event) {
        event.preventDefault();
        let isValid = true;

        if (usernameInput.value.length < 4) {
            usernameError.style.display = 'block';
            isValid = false;
        } else {
            usernameError.style.display = 'none';
        }

        if (passwordInput.value.length < 6) {
            passwordError.style.display = 'block';
            isValid = false;
        } else {
            passwordError.style.display = 'none';
        }

        if (passwordInput.value !== confirmPasswordInput.value) {
            confirmPasswordError.style.display = 'block';
            isValid = false;
        } else {
            confirmPasswordError.style.display = 'none';
        }

        if (isValid) {
            successMessage.style.display = 'block';
            form.reset();

            setTimeout(function () {
                successMessage.style.display = 'none';
            }, 3000);
        }
    });

    usernameInput.addEventListener('input', function () {
        if (usernameInput.value.length >= 4) {
            usernameError.style.display = 'none';
        }
    });

    passwordInput.addEventListener('input', function () {
        if (passwordInput.value.length >= 6) {
            passwordError.style.display = 'none';
        }

        if (confirmPasswordInput.value && passwordInput.value === confirmPasswordInput.value) {
            confirmPasswordError.style.display = 'none';
        }
    });

    confirmPasswordInput.addEventListener('input', function () {
        if (passwordInput.value === confirmPasswordInput.value) {
            confirmPasswordError.style.display = 'none';
        }
    });
});