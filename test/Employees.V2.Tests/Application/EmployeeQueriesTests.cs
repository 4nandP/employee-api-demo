using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using NUnit.Framework;

namespace Employees.Tests.V2.Application
{
    public class EmployeeQueriesTests
    {
        private Employees.V2.Application.Queries.IEmployeeQueries _query;
        private JsonFlatFileDataStore.IDataStore _store;
        private ILogger<Employees.V2.Application.Queries.EmployeeQueries> _logger;
        private readonly Infrastructure.Data.Entities.Employee _expectedEmployee;

        public EmployeeQueriesTests()
        {
            _expectedEmployee = new Infrastructure.Data.Entities.Employee
            {
                IsOrganization = false,
                Title = "Mrs.",
                GivenName = "Jane",
                MiddleName = "Lane",
                FamilyName = "Doe",
                DisplayName = "Jane Lane Doe",
                PrintOnCheckName = "Jane Lane Doe",
                IsActive = true,
                PrimaryPhone = new Employees.Infrastructure.Data.Entities.Phone { FreeFormNumber = "505.555.9999" },
                PrimaryEmailAddress = new Employees.Infrastructure.Data.Entities.EmailAddress { Address = "user@example.com" },
                EmployeeType = "Regular",
                Status = "Active",
                Id = "Test123"
            };
        }

        [SetUp]
        public void Setup()
        {
            _logger = A.Fake<ILogger<Employees.V2.Application.Queries.EmployeeQueries>>();
            _store = A.Fake<JsonFlatFileDataStore.IDataStore>();
            var employees = new[]{ _expectedEmployee }.ToList();

            var employeesCollection = A.Fake<JsonFlatFileDataStore.IDocumentCollection<Employees.Infrastructure.Data.Entities.Employee>>();
            A.CallTo(() => employeesCollection.AsQueryable()).Returns(employees);
            A.CallTo(() => _store.GetCollection<Employees.Infrastructure.Data.Entities.Employee>(null)).Returns(employeesCollection);
            _query = new Employees.V2.Application.Queries.EmployeeQueries(_store, _logger);
        }

        [Test(Description = "FindByIdAsync should throw exception when employee id is not provided")]
        public void FindByIdAsync_ShouldThrowArgumentNullException_WhenEmployeeIdIsNotProvided()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _query.FindByIdAsync(null, CancellationToken.None).ConfigureAwait(false));
            A.CallTo(() => _store.GetCollection<Employees.Infrastructure.Data.Entities.Employee>(null)).MustNotHaveHappened();
        }

        [Test(Description = "FindByIdAsync should return null when employee id does not match a record")]
        public async Task FindByIdAsync_ShouldReturnNull_WhenEmployeeIdDoesNotExist()
        {
            //Act
            var result = await _query.FindByIdAsync("FakeId", CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => _store.GetCollection<Employees.Infrastructure.Data.Entities.Employee>(null)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [Test(Description = "FindByIdAsync should return a record for a matching id")]
        public async Task FindByIdAsync_ShouldReturnAnEmployee_WhenEmployeeIdExists()
        {
            //Act
            var result = await _query.FindByIdAsync(_expectedEmployee.Id, CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => _store.GetCollection<Employees.Infrastructure.Data.Entities.Employee>(null)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(_expectedEmployee.Id, result.Id);
            Assert.AreEqual(_expectedEmployee.DisplayName, result.DisplayName);
            Assert.AreEqual(_expectedEmployee.EmployeeType, result.EmployeeType);
            Assert.AreEqual(_expectedEmployee.FamilyName, result.FamilyName);
            Assert.AreEqual(_expectedEmployee.GivenName, result.GivenName);
            Assert.AreEqual(_expectedEmployee.IsActive, result.IsActive);
            Assert.AreEqual(_expectedEmployee.IsOrganization, result.IsOrganization);
            Assert.AreEqual(_expectedEmployee.MiddleName, result.MiddleName);
            Assert.AreEqual(_expectedEmployee.PrimaryEmailAddress.Address, result.PrimaryEmailAddress);
            Assert.AreEqual(_expectedEmployee.PrimaryPhone.FreeFormNumber, result.PrimaryPhone);
            Assert.AreEqual(_expectedEmployee.PrintOnCheckName, result.PrintOnCheckName);
            Assert.AreEqual(_expectedEmployee.Status, result.Status);
            Assert.AreEqual(_expectedEmployee.Title, result.Title);
        }

        [Test(Description = "FindByIdAsync should short-circuit with null when cancelled")]
        public async Task FindByIdAsync_ShouldReturnNull_WhenCancelled()
        {
            using (CancellationTokenSource source = new CancellationTokenSource())
            {
                //Arrange
                var ct = source.Token;
                source.Cancel();

                //Act
                var result = await _query.FindByIdAsync("Test123", ct).ConfigureAwait(false);

                //Assert
                A.CallTo(() => _store.GetCollection<Employees.Infrastructure.Data.Entities.Employee>(null)).MustNotHaveHappened();
                Assert.IsNull(result);
            }
        }
    }
}