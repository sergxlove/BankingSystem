using BankingSystemCore.Models;

namespace BankingSystem.Tests.UnitTests
{
    public class ClientsTests
    {
        private Guid _validId;
        private const string _validFirstName = "Иван";
        private const string _validSecondName = "Иванович";
        private const string _validLastName = "Иванов";
        private DateOnly _validBirthDate;
        private const string _validPassportSeries = "1234";
        private const string _validPassportNumber = "567890";
        private const string _validPhoneNumber = "+79991234567";
        private const string _validEmail = "ivan@example.com";
        private const string _validAddress = "г. Москва, ул. Ленина, д. 1";
        private DateOnly _validDateRegistration;
        [SetUp]
        public void Setup()
        {
            _validId = Guid.NewGuid();
            _validBirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-20));
            _validDateRegistration = DateOnly.FromDateTime(DateTime.Today);
        }

        [Test]
        public void Create_WithValidData_ReturnsSuccess()
        {
            var result = Clients.Create(
                _validId, _validFirstName, _validSecondName, _validLastName,
                _validBirthDate, _validPassportSeries, _validPassportNumber,
                _validPhoneNumber, _validEmail, _validAddress, _validDateRegistration);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.Not.Null);
            });
        }

        [Test]
        public void Create_WithValidData_SetsPropertiesCorrectly()
        {
            var result = Clients.Create(
                _validId, _validFirstName, _validSecondName, _validLastName,
                _validBirthDate, _validPassportSeries, _validPassportNumber,
                _validPhoneNumber, _validEmail, _validAddress, _validDateRegistration);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(_validId));
                Assert.That(result.Value.FirstName, Is.EqualTo(_validFirstName));
                Assert.That(result.Value.LastName, Is.EqualTo(_validLastName));
                Assert.That(result.Value.PassportSeries, Is.EqualTo(_validPassportSeries));
                Assert.That(result.Value.PassportNumber, Is.EqualTo(_validPassportNumber));
                Assert.That(result.Value.PhoneNumber, Is.EqualTo(_validPhoneNumber));
            });
        }

        [Test]
        public void Create_WithEmptyId_ReturnsFailure()
        {
            var result = Clients.Create(
                Guid.Empty, _validFirstName, _validSecondName, _validLastName,
                _validBirthDate, _validPassportSeries, _validPassportNumber,
                _validPhoneNumber, _validEmail, _validAddress, _validDateRegistration);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Client ID cannot be empty"));
            });
        }

        [Test]
        public void Create_WithAgeUnder18_ReturnsFailure()
        {
            var underAgeBirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-17));
            var result = Clients.Create(
                _validId, _validFirstName, _validSecondName, _validLastName,
                underAgeBirthDate, _validPassportSeries, _validPassportNumber,
                _validPhoneNumber, _validEmail, _validAddress, _validDateRegistration);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Client must be at least 18 years old"));
            });
        }

        [Test]
        public void Create_WithExactly18Years_ShouldSucceed()
        {
            var exact18BirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-18));
            var result = Clients.Create(
                _validId, _validFirstName, _validSecondName, _validLastName,
                exact18BirthDate, _validPassportSeries, _validPassportNumber,
                _validPhoneNumber, _validEmail, _validAddress, _validDateRegistration);
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void Create_WithAgeOver18_ShouldSucceed()
        {
            var overAgeBirthDate = DateOnly.FromDateTime(DateTime.Today.AddYears(-25));
            var result = Clients.Create(
                _validId, _validFirstName, _validSecondName, _validLastName,
                overAgeBirthDate, _validPassportSeries, _validPassportNumber,
                _validPhoneNumber, _validEmail, _validAddress, _validDateRegistration);
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void Create_WithWhitespaceEmail_ShouldSucceed()
        {
            var result = Clients.Create(
                _validId, _validFirstName, _validSecondName, _validLastName,
                _validBirthDate, _validPassportSeries, _validPassportNumber,
                _validPhoneNumber, "", _validAddress, _validDateRegistration);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.EmailAddress, Is.EqualTo(""));
            });
        }

        [Test]
        public void Create_WithEmptySecondName_ShouldSucceed()
        {
            var result = Clients.Create(
                _validId, _validFirstName, "", _validLastName,
                _validBirthDate, _validPassportSeries, _validPassportNumber,
                _validPhoneNumber, _validEmail, _validAddress, _validDateRegistration);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.SecondName, Is.Empty);
            });
        }
    }
}
