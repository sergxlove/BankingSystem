using BankingSystemCore.Models;
using System.Runtime.InteropServices;

namespace BankingSystem.Tests.UnitTests
{
    public class CreditsTests
    {
        private Guid _validId;
        private Guid _validClientId;
        private Guid _validAccountId;
        private const decimal _validSumCredit = 100000m;
        private const int _validTermMonth = 12;
        private DateOnly _validStartDate;
        private DateOnly _validEndDate;
        private const decimal _validPaymentMonth = 9166.67m;
        private const decimal _validLeftCredit = 100000m;
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
            var result = Credits.Create(
                _validId, _validClientId, _validAccountId, _validSumCredit, _validTermMonth,
                _validStartDate, _validEndDate, _validPaymentMonth, _validLeftCredit, _validIsActive);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.Not.Null);
            });
        }

        [Test]
        public void Create_WithValidData_SetsPropertiesCorrectly()
        {
            var result = Credits.Create(
                _validId, _validClientId, _validAccountId, _validSumCredit, _validTermMonth,
                _validStartDate, _validEndDate, _validPaymentMonth, _validLeftCredit, _validIsActive);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(_validId));
                Assert.That(result.Value.ClientId, Is.EqualTo(_validClientId));
                Assert.That(result.Value.AccountId, Is.EqualTo(_validAccountId));
                Assert.That(result.Value.SumCredit, Is.EqualTo(_validSumCredit));
                Assert.That(result.Value.TermMonth, Is.EqualTo(_validTermMonth));
                Assert.That(result.Value.StartDate, Is.EqualTo(_validStartDate));
                Assert.That(result.Value.EndDate, Is.EqualTo(_validEndDate));
                Assert.That(result.Value.PaymentMonth, Is.EqualTo(_validPaymentMonth));
                Assert.That(result.Value.LeftCredit, Is.EqualTo(_validLeftCredit));
                Assert.That(result.Value.IsActive, Is.EqualTo(_validIsActive));
            });
        }

        [Test]
        public void Create_WithEmptyId_ReturnsSuccess()
        {
            var result = Credits.Create(
                Guid.Empty, _validClientId, _validAccountId, _validSumCredit, _validTermMonth,
                _validStartDate, _validEndDate, _validPaymentMonth, _validLeftCredit, _validIsActive);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.Id, Is.EqualTo(Guid.Empty));
            });
        }

        [Test]
        public void Create_WithZeroSumCredit_ReturnsSuccess()
        {
            var result = Credits.Create(
                _validId, _validClientId, _validAccountId, 0m, _validTermMonth,
                _validStartDate, _validEndDate, _validPaymentMonth, _validLeftCredit, _validIsActive);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.SumCredit, Is.EqualTo(0m));
            });
        }

        [Test]
        public void Create_WithZeroTermMonth_ReturnsSuccess()
        {
            var result = Credits.Create(
                _validId, _validClientId, _validAccountId, _validSumCredit, 0,
                _validStartDate, _validEndDate, _validPaymentMonth, _validLeftCredit, _validIsActive);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.TermMonth, Is.EqualTo(0));
            });
        }

        [Test]
        public void Create_WithIsActiveFalse_ReturnsSuccess()
        {
            var result = Credits.Create(
                _validId, _validClientId, _validAccountId, _validSumCredit, _validTermMonth,
                _validStartDate, _validEndDate, _validPaymentMonth, _validLeftCredit, false);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.IsActive, Is.False);
            });
        }

        [Test]
        public void GetPaymentsMonth_WithValidData_ReturnsCorrectValue()
        {
            var result = Credits.GetPaymentsMonth(100000m, 12);

            Assert.That(result, Is.EqualTo(9166.67m));
        }

        [Test]
        public void GetPaymentsMonth_WithDifferentSumCredit_ReturnsCorrectValue()
        {
            var result = Credits.GetPaymentsMonth(50000m, 10);

            Assert.That(result, Is.EqualTo(5500.00m));
        }

        [Test]
        public void GetPaymentsMonth_WithLargeSumCredit_ReturnsCorrectValue()
        {
            var result = Credits.GetPaymentsMonth(1000000m, 24);

            Assert.That(result, Is.EqualTo(51666.67m));
        }

        [Test]
        public void GetPaymentsMonth_WithOneMonthTerm_ReturnsCorrectValue()
        {
            var result = Credits.GetPaymentsMonth(10000m, 1);

            Assert.That(result, Is.EqualTo(10100.00m));
        }

        [Test]
        public void GetPaymentsMonth_ReturnsRoundedToTwoDecimals()
        {
            var result = Credits.GetPaymentsMonth(99999m, 12);

            Assert.That(result, Is.EqualTo(9333.24m));
        }
    }
}
