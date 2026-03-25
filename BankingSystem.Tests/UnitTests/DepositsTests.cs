using BankingSystemCore.Models;

namespace BankingSystem.Tests.UnitTests
{
    public class DepositsTests
    {
        private Guid _validId;
        private Guid _validClientId;
        private Guid _validAccountId;
        private const decimal _validSumDeposit = 50000m;
        private const int _validTermMonth = 12;
        private DateOnly _validStartDate;
        private DateOnly _validEndDate;
        private const int _validPercentYear = 15;
        private const bool _validIsActive = true;
        [SetUp]
        public void Setup()
        {
            _validId = Guid.NewGuid();
            _validClientId = Guid.NewGuid();
            _validAccountId = Guid.NewGuid();
            _validStartDate = DateOnly.FromDateTime(DateTime.Today);
            _validEndDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(12));
        }

        [Test]
        public void Create_WithValidData_ReturnsSuccess()
        {
            var result = Deposits.Create(
                _validId, _validClientId, _validAccountId, _validSumDeposit, _validTermMonth,
                _validStartDate, _validEndDate, _validPercentYear, _validIsActive);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.Not.Null);
            });
        }

        [Test]
        public void Create_WithValidData_SetsPropertiesCorrectly()
        {
            var result = Deposits.Create(
                _validId, _validClientId, _validAccountId, _validSumDeposit, _validTermMonth,
                _validStartDate, _validEndDate, _validPercentYear, _validIsActive);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(_validId));
                Assert.That(result.Value.ClientId, Is.EqualTo(_validClientId));
                Assert.That(result.Value.AccountId, Is.EqualTo(_validAccountId));
                Assert.That(result.Value.SumDeposit, Is.EqualTo(_validSumDeposit));
                Assert.That(result.Value.TermMonth, Is.EqualTo(_validTermMonth));
                Assert.That(result.Value.StartDate, Is.EqualTo(_validStartDate));
                Assert.That(result.Value.EndDate, Is.EqualTo(_validEndDate));
                Assert.That(result.Value.PercentYear, Is.EqualTo(_validPercentYear));
                Assert.That(result.Value.IsActive, Is.EqualTo(_validIsActive));
            });
        }

        [Test]
        public void Create_WithIsActiveFalse_ReturnsSuccess()
        {
            var result = Deposits.Create(
                _validId, _validClientId, _validAccountId, _validSumDeposit, _validTermMonth,
                _validStartDate, _validEndDate, _validPercentYear, false);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.IsActive, Is.False);
            });
        }

        [Test]
        public void GetCurrentPercentYear_ReturnsFifteen()
        {
            var result = Deposits.GetCurrentPercentYear();

            Assert.That(result, Is.EqualTo(15));
        }

        [Test]
        public void GetCurrentPercentYear_AlwaysReturnsSameValue()
        {
            var firstResult = Deposits.GetCurrentPercentYear();
            var secondResult = Deposits.GetCurrentPercentYear();

            Assert.That(firstResult, Is.EqualTo(secondResult));
        }

    }

}
