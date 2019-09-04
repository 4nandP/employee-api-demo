using FakeItEasy;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Employees.Tests.V1.Application
{
    public class EmployeeQueriesTests
    {
        private Employee.Api.V1.Application.Queries.IEmployeeQueries _query;
        private JsonFlatFileDataStore.IDataStore _store;

        [SetUp]
        public void Setup()
        {
            _store = A.Fake<JsonFlatFileDataStore.IDataStore>();
            var employees = new[]{new Employee.Infrastructure.Data.Entities.Employee
            {
                IsOrganization = false,
                Title = "Mrs.",
                GivenName = "Jane",
                MiddleName = "Lane",
                FamilyName = "Doe",
                DisplayName = "Jane Lane Doe",
                PrintOnCheckName = "Jane Lane Doe",
                IsActive = true,
                PrimaryPhone = new Employee.Infrastructure.Data.Entities.Phone { FreeFormNumber = "505.555.9999" },
                PrimaryEmailAddress = new Employee.Infrastructure.Data.Entities.EmailAddress { Address = "user@example.com" },
                EmployeeType = "Regular",
                Status = "Active",
                Id = "Test123"
            } }.ToList();

            var employeesCollection = A.Fake<JsonFlatFileDataStore.IDocumentCollection<Employee.Infrastructure.Data.Entities.Employee>>();
            A.CallTo(() => employeesCollection.AsQueryable()).Returns(employees);
            A.CallTo(() => _store.GetCollection<Employee.Infrastructure.Data.Entities.Employee>(null)).Returns(employeesCollection);
            _query = new Employee.Api.V1.Application.Queries.EmployeeQueries(_store);
        }

        [Test(Description = "FindByIdAsync should throw exception when employee id is not provided")]
        public void FindByIdAsync_ShouldThrowArgumentNullException_WhenEmployeeIdIsNotProvided()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _query.FindByIdAsync(null, CancellationToken.None).ConfigureAwait(false));
            A.CallTo(() => _store.GetCollection<Employee.Infrastructure.Data.Entities.Employee>(null)).MustHaveHappenedOnceExactly();
        }

        [Test(Description = "FindByIdAsync should return null when employee id does not match a record")]
        public async Task FindByIdAsync_ShouldReturnNull_WhenEmployeeIdDoesNotExist()
        {
            //Act
            var result = await _query.FindByIdAsync("FakeId", CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => _store.GetCollection<Employee.Infrastructure.Data.Entities.Employee>(null)).MustHaveHappenedOnceExactly();
            Assert.IsNull(result);
        }

        [Test(Description = "FindByIdAsync should return a record for a matching id")]
        public async Task FindByIdAsync_ShouldReturnAnEmployee_WhenEmployeeIdExists()
        {
            //Act
            var result = await _query.FindByIdAsync("Test123", CancellationToken.None).ConfigureAwait(false);

            //Assert
            A.CallTo(() => _store.GetCollection<Employee.Infrastructure.Data.Entities.Employee>(null)).MustHaveHappenedOnceExactly();
            Assert.AreEqual(result.Employee.Id, "Test123");
        }
    }
}