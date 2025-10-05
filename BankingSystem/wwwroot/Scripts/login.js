﻿document.addEventListener('DOMContentLoaded', function () {
    const form = document.querySelector('form');
    const submitButton = form.querySelector('button[type="submit"]');
    const spinner = document.createElement('div');
    spinner.className = 'spinner';
    spinner.style.display = 'none';
    spinner.innerHTML = `
        <div style="
            border: 3px solid #f3f3f3;
            border-top: 3px solid #3498db;
            border-radius: 50%;
            width: 20px;
            height: 20px;
            animation: spin 1s linear infinite;
            margin: 0 auto;
        "></div>
        <style>
            @keyframes spin {
                0% { transform: rotate(0deg); }
                100% { transform: rotate(360deg); }
            }
        </style>
    `;

    submitButton.parentNode.insertBefore(spinner, submitButton.nextSibling);

    form.addEventListener('submit', async function (event) {
        event.preventDefault();

        const username = document.getElementById('username').value;
        const password = document.getElementById('password').value;

        showLoadingState();

        try {
            const response = await fetch('/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({
                    Username: username,
                    Password: password
                })
            });

            switch (response.status) {
                case 200:
                    window.location.href = "../Pages/index.html";
                    break;
                default:
                    handleError('Ошибка авторизации');
                    break;
            }
        }
        catch (error) {
            handleError('Ошибка соединения с сервером');
        }
        finally {
            hideLoadingState();
        }
    });

    function showLoadingState() {
        submitButton.disabled = true;
        submitButton.textContent = 'Выполняется вход...';
        spinner.style.display = 'block';
    }

    function hideLoadingState() {
        submitButton.disabled = false;
        submitButton.textContent = 'Войти';
        spinner.style.display = 'none';
    }


    function handleError(errorMessage) {
        clearErrors();
        showMessage(errorMessage, 'error');
        const inputs = document.querySelectorAll('input');
        inputs.forEach(input => {
            input.style.borderColor = '#e74c3c';
        });
    }

    function showMessage(message, type) {
        const existingMessage = document.querySelector('.message');
        if (existingMessage) {
            existingMessage.remove();
        }
        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${type}`;
        messageDiv.textContent = message;
        messageDiv.style.cssText = `
            padding: 10px;
            margin: 10px 0;
            border-radius: 4px;
            text-align: center;
            font-weight: bold;
            ${type === 'success' ?
                'background-color: #d4edda; color: #155724; border: 1px solid #c3e6cb;' :
                'background-color: #f8d7da; color: #721c24; border: 1px solid #f5c6cb;'
            }
        `;

        form.parentNode.insertBefore(messageDiv, form);
        if (type === 'error') {
            setTimeout(() => {
                messageDiv.remove();
            }, 5000);
        }
    }

    function clearErrors() {
        const existingMessage = document.querySelector('.message');
        if (existingMessage) {
            existingMessage.remove();
        }

        const inputs = document.querySelectorAll('input');
        inputs.forEach(input => {
            input.style.borderColor = '';
        });
    }
});