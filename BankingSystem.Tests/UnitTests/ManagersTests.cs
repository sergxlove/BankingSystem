using BankingSystemCore.Models;

namespace BankingSystem.Tests.UnitTests
{
    public class ManagersTests
    {
        private Guid _validId;
        private const string _validClientSeries = "1234";
        private const string _validClientNumbers = "567890";
        private const string _validLoginManager = "manager123";
        [SetUp]
        public void SetUp()
        {
            _validId = Guid.NewGuid();
        }

        [Test]
        public void Create_WithValidData_ReturnsSuccess()
        {
            var result = Managers.Create(_validId, _validClientSeries, _validClientNumbers, _validLoginManager);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.Not.Null);
            });
        }

        [Test]
        public void Create_WithValidData_SetsPropertiesCorrectly()
        {
            var result = Managers.Create(_validId, _validClientSeries, _validClientNumbers, _validLoginManager);

            Assert.Multiple(() =>
            {
                Assert.That(result.Value.Id, Is.EqualTo(_validId));
                Assert.That(result.Value.ClientSeries, Is.EqualTo(_validClientSeries));
                Assert.That(result.Value.ClientNumbers, Is.EqualTo(_validClientNumbers));
                Assert.That(result.Value.LoginManager, Is.EqualTo(_validLoginManager));
            });
        }

        [Test]
        public void Create_WithEmptyId_ReturnsFailure()
        {
            var result = Managers.Create(Guid.Empty, _validClientSeries, _validClientNumbers, _validLoginManager);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Manager ID cannot be empty"));
                Assert.That(result.Value, Is.Null);
            });
        }

        [Test]
        public void Create_WithNullClientSeries_ReturnsFailure()
        {
            var result = Managers.Create(_validId, string.Empty, _validClientNumbers, _validLoginManager);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Client series is required"));
            });
        }

        [Test]
        public void Create_WithEmptyClientSeries_ReturnsFailure()
        {
            var result = Managers.Create(_validId, string.Empty, _validClientNumbers, _validLoginManager);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Client series is required"));
            });
        }

        [Test]
        public void Create_WithWhitespaceClientSeries_ReturnsFailure()
        {
            var result = Managers.Create(_validId, "   ", _validClientNumbers, _validLoginManager);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Client series is required"));
            });
        }

        [Test]
        public void Create_WithNullClientNumbers_ReturnsFailure()
        {
            var result = Managers.Create(_validId, _validClientSeries, string.Empty, _validLoginManager);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Client numbers is required"));
            });
        }

        [Test]
        public void Create_WithEmptyClientNumbers_ReturnsFailure()
        {
            var result = Managers.Create(_validId, _validClientSeries, string.Empty, _validLoginManager);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Client numbers is required"));
            });
        }

        [Test]
        public void Create_WithWhitespaceClientNumbers_ReturnsFailure()
        {
            var result = Managers.Create(_validId, _validClientSeries, "   ", _validLoginManager);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Client numbers is required"));
            });
        }

        [Test]
        public void Create_WithNullLoginManager_ReturnsFailure()
        {
            var result = Managers.Create(_validId, _validClientSeries, _validClientNumbers, string.Empty);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Login is required"));
            });
        }

        [Test]
        public void Create_WithEmptyLoginManager_ReturnsFailure()
        {
            var result = Managers.Create(_validId, _validClientSeries, _validClientNumbers, string.Empty);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Login is required"));
            });
        }

        [Test]
        public void Create_WithWhitespaceLoginManager_ReturnsFailure()
        {
            var result = Managers.Create(_validId, _validClientSeries, _validClientNumbers, "   ");

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Login is required"));
            });
        }

        [Test]
        public void Create_WithMultipleInvalidFields_ReturnsFirstValidationError()
        {
            var result = Managers.Create(Guid.Empty, string.Empty, string.Empty, string.Empty);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Error, Is.EqualTo("Manager ID cannot be empty"));
            });
        }
    }
}
