using BankingSystemCore.Models;

namespace BankingSystem.Tests.UnitTests
{
    public class OperationsTransactionsTests
    {
        private Guid _validId;
        private const string _validTypeOperation = "Transfer";
        private const string _validDescription = "Money transfer between accounts";

        [SetUp]
        public void SetUp()
        {
            _validId = Guid.NewGuid();
        }

        [Test]
        public void Create_WithValidData_ReturnsSuccess()
        {
            var result = OperationsTransactions.Create(_validId, _validTypeOperation, _validDescription);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.Not.Null);
            });
        }

        [Test]
        public void Create_WithValidData_SetsPropertiesCorrectly()
        {
            var result = OperationsTransactions.Create(_validId, _validTypeOperation, _validDescription);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(_validId));
                Assert.That(result.Value.TypeOperation, Is.EqualTo(_validTypeOperation));
                Assert.That(result.Value.Description, Is.EqualTo(_validDescription));
            });
        }

        [Test]
        public void Create_WithValidDataAndEmptyDescription_SetsDescriptionToEmpty()
        {
            var result = OperationsTransactions.Create(_validId, _validTypeOperation, string.Empty);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.Description, Is.Empty);
            });
        }

        [Test]
        public void Create_WithEmptyId_ReturnsFailure()
        {
            var result = OperationsTransactions.Create(Guid.Empty, _validTypeOperation, _validDescription);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Operation ID cannot be empty"));
                Assert.That(result.Value, Is.Null);
            });
        }

        [Test]
        public void Create_WithEmptyTypeOperation_ReturnsFailure()
        {
            var result = OperationsTransactions.Create(_validId, string.Empty, _validDescription);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Operation type is required"));
            });
        }

        [Test]
        public void Create_WithWhitespaceTypeOperation_ReturnsFailure()
        {
            var result = OperationsTransactions.Create(_validId, "   ", _validDescription);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Operation type is required"));
            });
        }
    }
}
