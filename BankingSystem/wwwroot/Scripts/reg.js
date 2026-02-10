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

    form.addEventListener('submit', async function (event) {
        event.preventDefault();
        let isValid = true;

        // Сброс сообщений об ошибках
        usernameError.style.display = 'none';
        passwordError.style.display = 'none';
        confirmPasswordError.style.display = 'none';
        successMessage.style.display = 'none';

        // Валидация
        if (usernameInput.value.length < 4) {
            usernameError.style.display = 'block';
            isValid = false;
        }

        if (passwordInput.value.length < 6) {
            passwordError.style.display = 'block';
            isValid = false;
        }

        if (passwordInput.value !== confirmPasswordInput.value) {
            confirmPasswordError.style.display = 'block';
            isValid = false;
        }

        if (!isValid) {
            return;
        }

        // Подготовка данных для отправки
        const formData = {
            username: usernameInput.value,
            password: passwordInput.value
        };

        try {
            // Отправка данных на сервер
            const response = await fetch('/api/register', { // Замените на ваш URL API
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(formData)
            });

            if (response.ok) {
                // Успешная регистрация
                successMessage.textContent = 'Регистрация прошла успешно!';
                successMessage.style.display = 'block';
                successMessage.style.color = 'green';
                form.reset();

                // Перенаправление через 2 секунды
                setTimeout(function () {
                    window.location.href = 'index.html';
                }, 2000);
            } else {
                // Обработка ошибок сервера
                const errorData = await response.json();
                showServerError(errorData.message || 'Ошибка сервера');
            }
        } catch (error) {
            console.error('Ошибка:', error);
            showServerError('Ошибка подключения к серверу');
        }
    });

    function showServerError(message) {
        successMessage.textContent = message;
        successMessage.style.display = 'block';
        successMessage.style.color = 'red';

        setTimeout(function () {
            successMessage.style.display = 'none';
        }, 5000);
    }

    // Live валидация (оставляем без изменений)
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