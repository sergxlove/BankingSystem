using BankingSystemCore.Models;

namespace BankingSystem.Tests.UnitTests
{
    public class TransactionsTests
    {
        private Guid _validId;
        private Guid _validProducerAccount;
        private Guid _validConsumerAccount;
        private Guid _validTypeOperation;
        private const decimal _validAmount = 1000.50m;
        private const string _validDescription = "Payment for services";
        private DateOnly _validDateCreated;

        [SetUp]
        public void SetUp()
        {
            _validId = Guid.NewGuid();
            _validProducerAccount = Guid.NewGuid();
            _validConsumerAccount = Guid.NewGuid();
            _validTypeOperation = Guid.NewGuid();
            _validDateCreated = DateOnly.FromDateTime(DateTime.Today);
        }

        [Test]
        public void Create_WithValidData_ReturnsSuccess()
        {
            var result = Transactions.Create(
                _validId, _validProducerAccount, _validConsumerAccount, _validTypeOperation,
                _validAmount, _validDescription, _validDateCreated);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.Not.Null);
            });
        }

        [Test]
        public void Create_WithValidData_SetsPropertiesCorrectly()
        {
            var result = Transactions.Create(
                _validId, _validProducerAccount, _validConsumerAccount, _validTypeOperation,
                _validAmount, _validDescription, _validDateCreated);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(_validId));
                Assert.That(result.Value.ProducerAccount, Is.EqualTo(_validProducerAccount));
                Assert.That(result.Value.ConsumerAccount, Is.EqualTo(_validConsumerAccount));
                Assert.That(result.Value.TypeOperation, Is.EqualTo(_validTypeOperation));
                Assert.That(result.Value.Amount, Is.EqualTo(_validAmount));
                Assert.That(result.Value.Description, Is.EqualTo(_validDescription));
                Assert.That(result.Value.DateCreated, Is.EqualTo(_validDateCreated));
            });
        }

        [Test]
        public void Create_WithValidDataAndEmptyDescription_SetsDescriptionToEmpty()
        {
            var result = Transactions.Create(
                _validId, _validProducerAccount, _validConsumerAccount, _validTypeOperation,
                _validAmount, string.Empty, _validDateCreated);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.Description, Is.Empty);
            });
        }

        [Test]
        public void Create_WithEmptyId_ReturnsFailure()
        {
            var result = Transactions.Create(
                Guid.Empty, _validProducerAccount, _validConsumerAccount, _validTypeOperation,
                _validAmount, _validDescription, _validDateCreated);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Transaction ID cannot be empty"));
                Assert.That(result.Value, Is.Null);
            });
        }

        [Test]
        public void Create_WithEmptyProducerAccount_ReturnsFailure()
        {
            var result = Transactions.Create(
                _validId, Guid.Empty, _validConsumerAccount, _validTypeOperation,
                _validAmount, _validDescription, _validDateCreated);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Producer account ID cannot be empty"));
            });
        }

        [Test]
        public void Create_WithEmptyConsumerAccount_ReturnsFailure()
        {
            var result = Transactions.Create(
                _validId, _validProducerAccount, Guid.Empty, _validTypeOperation,
                _validAmount, _validDescription, _validDateCreated);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Consumer account ID cannot be empty"));
            });
        }

        [Test]
        public void Create_WithEmptyTypeOperation_ReturnsFailure()
        {
            var result = Transactions.Create(
                _validId, _validProducerAccount, _validConsumerAccount, Guid.Empty,
                _validAmount, _validDescription, _validDateCreated);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Operation type ID cannot be empty"));
            });
        }

        [Test]
        public void Create_WithZeroAmount_ReturnsFailure()
        {
            var result = Transactions.Create(
                _validId, _validProducerAccount, _validConsumerAccount, _validTypeOperation,
                0m, _validDescription, _validDateCreated);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Amount must be greater than zero"));
            });
        }

        [Test]
        public void Create_WithNegativeAmount_ReturnsFailure()
        {
            var result = Transactions.Create(
                _validId, _validProducerAccount, _validConsumerAccount, _validTypeOperation,
                -500m, _validDescription, _validDateCreated);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Amount must be greater than zero"));
            });
        }

        [Test]
        public void Create_WithDefaultDateCreated_ReturnsSuccess()
        {
            var result = Transactions.Create(
                _validId, _validProducerAccount, _validConsumerAccount, _validTypeOperation,
                _validAmount, _validDescription, default);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.DateCreated, Is.EqualTo(default(DateOnly)));
            });
        }

        [Test]
        public void Create_WithProducerAndConsumerSameAccount_ReturnsSuccess()
        {
            var sameAccount = Guid.NewGuid();

            var result = Transactions.Create(
                _validId, sameAccount, sameAccount, _validTypeOperation,
                _validAmount, _validDescription, _validDateCreated);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value.ProducerAccount, Is.EqualTo(result.Value.ConsumerAccount));
            });
        }
    }
}
