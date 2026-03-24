using BankingSystemCore.Models;

namespace BankingSystem.Tests.UnitTests
{
    public class AccountsTests
    {
        private Guid _validId;
        private Guid _validClientId;
        private const string _validAccountType = "Checking";
        private const string _validAccountNumber = "1234567890";
        private const decimal _validBalance = 1000.50m;
        private const string _validCurrencyCode = "USD";
        private DateOnly _validOpenDate;
        private DateOnly _validCloseDate;
        private const bool _validIsActive = true;
        [SetUp]
        public void Setup()
        {
            _validId = Guid.NewGuid();
            _validClientId = Guid.NewGuid();
            _validOpenDate = DateOnly.FromDateTime(DateTime.Today);
            _validCloseDate = DateOnly.FromDateTime(DateTime.Today.AddYears(1));
        }

        [Test]
        public void Create_WithValidParameters_ReturnsSuccessResult()
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                _validAccountType,
                _validAccountNumber,
                _validBalance,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.Error, Is.Null.Or.Empty);
            });
        }

        [Test]
        public void Create_WithValidParameters_SetsPropertiesCorrectly()
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                _validAccountType,
                _validAccountNumber,
                _validBalance,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(_validId));
                Assert.That(result.Value.ClientsId, Is.EqualTo(_validClientId));
                Assert.That(result.Value.AccountType, Is.EqualTo(_validAccountType));
                Assert.That(result.Value.AccountNumber, Is.EqualTo(_validAccountNumber));
                Assert.That(result.Value.Balance, Is.EqualTo(_validBalance));
                Assert.That(result.Value.CurrencyCode, Is.EqualTo(_validCurrencyCode));
                Assert.That(result.Value.OpenDate, Is.EqualTo(_validOpenDate));
                Assert.That(result.Value.CloseDate, Is.EqualTo(_validCloseDate));
                Assert.That(result.Value.IsActive, Is.EqualTo(_validIsActive));
            });
        }

        [Test]
        public void Create_WithEmptyId_ReturnsFailureResult()
        {
            var result = Accounts.Create(
                Guid.Empty,
                _validClientId,
                _validAccountType,
                _validAccountNumber,
                _validBalance,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Error, Is.EqualTo("Account ID cannot be empty"));
            });
        }

        [Test]
        public void Create_WithEmptyClientId_ReturnsFailureResult()
        {
            var result = Accounts.Create(
                _validId,
                Guid.Empty,
                _validAccountType,
                _validAccountNumber,
                _validBalance,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Error, Is.EqualTo("Client ID cannot be empty"));
            });
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Create_WithInvalidAccountType_ReturnsFailureResult(string invalidAccountType)
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                invalidAccountType,
                _validAccountNumber,
                _validBalance,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Error, Is.EqualTo("Account type is required"));
            });
        }

        [Test]
        public void Create_WithWhitespaceAccountType_ReturnsFailureResult()
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                "   ",
                _validAccountNumber,
                _validBalance,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Account type is required"));
            });
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Create_WithInvalidAccountNumber_ReturnsFailureResult(string invalidAccountNumber)
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                _validAccountType,
                invalidAccountNumber,
                _validBalance,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Error, Is.EqualTo("Account number is required"));
            });
        }

        [TestCase("")]
        [TestCase("   ")]
        public void Create_WithInvalidCurrencyCode_ReturnsFailureResult(string invalidCurrencyCode)
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                _validAccountType,
                _validAccountNumber,
                _validBalance,
                invalidCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Error, Is.EqualTo("Currency code is required"));
            });
        }

        [Test]
        public void Create_WithZeroBalance_ShouldSucceed()
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                _validAccountType,
                _validAccountNumber,
                0m,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.Balance, Is.EqualTo(0m));
            });
        }

        [Test]
        public void Create_WithNegativeBalance_ShouldSucceed()
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                _validAccountType,
                _validAccountNumber,
                -500.75m,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.Balance, Is.EqualTo(-500.75m));
            });
        }

        [Test]
        public void Create_WithOpenDateInFuture_ShouldSucceed()
        {
            var futureDate = DateOnly.FromDateTime(DateTime.Today.AddYears(1));
            var result = Accounts.Create(
                _validId,
                _validClientId,
                _validAccountType,
                _validAccountNumber,
                _validBalance,
                _validCurrencyCode,
                futureDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.OpenDate, Is.EqualTo(futureDate));
            });
        }

        [Test]
        public void Create_WithCloseDateEarlierThanOpenDate_ShouldSucceed()
        {
            var openDate = DateOnly.FromDateTime(DateTime.Today);
            var closeDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
            var result = Accounts.Create(
                _validId,
                _validClientId,
                _validAccountType,
                _validAccountNumber,
                _validBalance,
                _validCurrencyCode,
                openDate,
                closeDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.OpenDate, Is.EqualTo(openDate));
                Assert.That(result.Value.CloseDate, Is.EqualTo(closeDate));
            });
        }

        [Test]
        public void Create_WithIsActiveFalse_ShouldSucceed()
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                _validAccountType,
                _validAccountNumber,
                _validBalance,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                false);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.IsActive, Is.False);
            });
        }

        [Test]
        public void Create_WithEmptyIdAndEmptyClientId_ReturnsIdValidationErrorFirst()
        {
            var result = Accounts.Create(
                Guid.Empty,
                Guid.Empty,
                _validAccountType,
                _validAccountNumber,
                _validBalance,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.That(result.Error, Is.EqualTo("Account ID cannot be empty"));
        }

        [TestCase("1234567890")]
        [TestCase("A1B2C3D4E5")]
        [TestCase("123-456-789")]
        [TestCase("123.456.789/0")]
        [TestCase("12345678901234567890")] 
        [TestCase("1")] 
        public void Create_WithVariousValidAccountNumbers_ShouldSucceed(string accountNumber)
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                _validAccountType,
                accountNumber,
                _validBalance,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.AccountNumber, Is.EqualTo(accountNumber));
            });
        }

        [TestCase("USD")]
        [TestCase("EUR")]
        [TestCase("GBP")]
        [TestCase("JPY")]
        [TestCase("RUB")]
        [TestCase("usd")] 
        [TestCase("UsD")] 
        public void Create_WithVariousCurrencyCodes_ShouldSucceed(string currencyCode)
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                _validAccountType,
                _validAccountNumber,
                _validBalance,
                currencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.CurrencyCode, Is.EqualTo(currencyCode));
            });
        }

        [TestCase("Checking")]
        [TestCase("Savings")]
        [TestCase("Business")]
        [TestCase("Credit")]
        [TestCase("Investment")]
        [TestCase("checking")] 
        [TestCase("  Checking  ")] 
        public void Create_WithVariousAccountTypes_ShouldSucceed(string accountType)
        {
            var result = Accounts.Create(
                _validId,
                _validClientId,
                accountType,
                _validAccountNumber,
                _validBalance,
                _validCurrencyCode,
                _validOpenDate,
                _validCloseDate,
                _validIsActive);
            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.AccountType, Is.EqualTo(accountType));
            });
        }
    }
}
