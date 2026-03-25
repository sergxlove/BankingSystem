using BankingSystemCore.Abstractions;
using BankingSystemCore.Models;
using BankingSystemCore.Services;

namespace BankingSystem.Tests.UnitTests
{
    public class UsersTests
    {
        private IPasswordHasherService _mockPasswordHasher;
        private Guid _validId;
        private const string _validUsername = "john_doe";
        private const string _validPassword = "SecurePass123";
        private const string _validHash = "hashed_password_123";
        private const string _validRole = "Admin";

        [SetUp]
        public void SetUp()
        {
            _validId = Guid.NewGuid();
            _mockPasswordHasher = new PasswordHasherService();
        }

        [Test]
        public void Create_WithValidData_ReturnsSuccess()
        {
            var result = Users.Create(
                _validId, _validUsername, _validPassword, _validRole, _mockPasswordHasher);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.Not.Null);
            });
        }

        [Test]
        public void Create_WithEmptyId_ReturnsFailure()
        {
            var result = Users.Create(
                Guid.Empty, _validUsername, _validPassword, _validRole, _mockPasswordHasher);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Поле Id не должно быть пустым"));
                Assert.That(result.Value, Is.Null);
            });
        }

        [Test]
        public void Create_WithEmptyUsername_ReturnsFailure()
        {
            var result = Users.Create(
                _validId, string.Empty, _validPassword, _validRole, _mockPasswordHasher);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Поле Имя не должно быть пустым"));
            });
        }

        [Test]
        public void Create_WithNullUsername_ReturnsFailure()
        {
            var result = Users.Create(
                _validId, "", _validPassword, _validRole, _mockPasswordHasher);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Поле Имя не должно быть пустым"));
            });
        }

        [Test]
        public void Create_WithWhitespaceUsername_ReturnsFailure()
        {
            var result = Users.Create(
                _validId, "   ", _validPassword, _validRole, _mockPasswordHasher);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Поле Имя не должно быть пустым"));
            });
        }

        [Test]
        public void Create_WithEmptyPassword_ReturnsFailure()
        {
            var result = Users.Create(
                _validId, _validUsername, string.Empty, _validRole, _mockPasswordHasher);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Поле Пароль не должно быть пустым"));
            });
        }

        [Test]
        public void Create_WithNullPassword_ReturnsFailure()
        {
            var result = Users.Create(
                _validId, _validUsername, "", _validRole, _mockPasswordHasher);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Поле Пароль не должно быть пустым"));
            });
        }

        [Test]
        public void Create_WithEmptyRole_ReturnsFailure()
        {
            var result = Users.Create(
                _validId, _validUsername, _validPassword, string.Empty, _mockPasswordHasher);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Поле Роль не должно быть пустым"));
            });
        }

        [Test]
        public void Create_WithNullRole_ReturnsFailure()
        {
            var result = Users.Create(
                _validId, _validUsername, _validPassword, "", _mockPasswordHasher);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Поле Роль не должно быть пустым"));
            });
        }

    }
}
