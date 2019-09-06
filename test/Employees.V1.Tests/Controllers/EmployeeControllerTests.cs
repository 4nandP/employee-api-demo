using System.Threading;
using System.Threading.Tasks;
using Employees.V1.Application.Queries;
using Employees.V1.Controllers;
using Employees.V1.Domain;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Employees.Tests.V1.Controllers
{
    public class EmployeeControllerTests
    {
        private IEmployeeQueries _query;
        private EmployeeController _controller;
        private readonly Employee _expectedEmployee;

        public EmployeeControllerTests()
        {
            _expectedEmployee = new Employee
            {
                Organization = false,
                Title = "Mrs.",
                GivenName = "Jane",
                MiddleName = "Lane",
                FamilyName = "Doe",
                DisplayName = "Jane Lane Doe",
                PrintOnCheckName = "Jane Lane Doe",
                Active = true,
                PrimaryPhone = new Phone { FreeFormNumber = "505.555.9999" },
                PrimaryEmailAddress = new EmailAddress { Address = "user@example.com" },
                EmployeeType = "Regular",
                Status = "Active",
                Id = "Test123"
            };
        }

        [SetUp]
        public void Setup()
        {
            _query = A.Fake<IEmployeeQueries>();

            A.CallTo(() => _query.FindByIdAsync(A<string>.Ignored, A<CancellationToken>.Ignored)).Returns(Task.FromResult<EmployeeQueryResponse>(null));
            A.CallTo(() => _query.FindByIdAsync("Test123", A<CancellationToken>.Ignored)).Returns(Task.FromResult(new EmployeeQueryResponse(_expectedEmployee)));

            _controller = new EmployeeController(_query);
        }

        [Test]
        public async Task GetDetails_ShouldReturnBadRequest_WhenIdIsNotProvided()
        {
            var result = await _controller.GetDetails(null, CancellationToken.None).ConfigureAwait(false);

            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            Assert.AreEqual("id", ((BadRequestObjectResult)result).Value);
        }

        [Test]
        public async Task GetDetails_ShouldReturnNotFound_WhenIdDoesNotMatchAnEmployee()
        {
            var result = await _controller.GetDetails("Noone", CancellationToken.None).ConfigureAwait(false);

            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task GetDetails_ShouldReturnOk_WhenIdMatchesAnEmployee()
        {
            var result = await _controller.GetDetails("Test123", CancellationToken.None).ConfigureAwait(false);

            Assert.IsInstanceOf<OkObjectResult>(result);
            var t = (OkObjectResult)result;
            Assert.IsInstanceOf<QueryResponse<EmployeeQueryResponse>>(t.Value);
            var response = t.Value as QueryResponse<EmployeeQueryResponse>;
            var payload = response.QueryResponseData;
            Assert.NotNull(payload);
            var employee = payload.Employee;
            Assert.NotNull(employee);
            Assert.AreEqual(1, payload.MaxResults);
            Assert.AreEqual(1, payload.StartPosition);
            Assert.AreEqual(_expectedEmployee.Active, employee.Active);
            Assert.AreEqual(_expectedEmployee.DisplayName, employee.DisplayName);
            Assert.AreEqual(_expectedEmployee.EmployeeType, employee.EmployeeType);
            Assert.AreEqual(_expectedEmployee.FamilyName, employee.FamilyName);
            Assert.AreEqual(_expectedEmployee.GivenName, employee.GivenName);
            Assert.AreEqual(_expectedEmployee.Id, employee.Id);
            Assert.AreEqual(_expectedEmployee.MiddleName, employee.MiddleName);
            Assert.AreEqual(_expectedEmployee.Organization, employee.Organization);
            Assert.AreEqual(_expectedEmployee.PrimaryEmailAddress, employee.PrimaryEmailAddress);
            Assert.AreEqual(_expectedEmployee.PrimaryPhone, employee.PrimaryPhone);
            Assert.AreEqual(_expectedEmployee.PrintOnCheckName, employee.PrintOnCheckName);
            Assert.AreEqual(_expectedEmployee.Status, employee.Status);
            Assert.AreEqual(_expectedEmployee.Title, employee.Title);
        }
    }
}
