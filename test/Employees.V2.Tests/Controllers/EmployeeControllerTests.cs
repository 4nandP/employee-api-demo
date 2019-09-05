using Employees.V2.Application.Queries;
using Employees.V2.Controllers;
using Employees.V2.Domain;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Employees.Tests.V2.Controllers
{
    public class EmployeeControllerTests
    {
        private IEmployeeQueries _query;
        private EmployeesController _controller;
        private ILogger<EmployeesController> _logger;

        [SetUp]
        public void Setup()
        {
            _query = A.Fake<IEmployeeQueries>();
            _logger = A.Fake<ILogger<EmployeesController>>();

            A.CallTo(() => _query.FindByIdAsync(A<string>.Ignored, A<CancellationToken>.Ignored)).Returns(Task.FromResult<Employee>(null));
            A.CallTo(() => _query.FindByIdAsync("Test123", A<CancellationToken>.Ignored)).Returns(Task.FromResult(new Employee
            {
                IsOrganization = false,
                Title = "Mrs.",
                GivenName = "Jane",
                MiddleName = "Lane",
                FamilyName = "Doe",
                DisplayName = "Jane Lane Doe",
                PrintOnCheckName = "Jane Lane Doe",
                IsActive = true,
                PrimaryPhone = "505.555.9999",
                PrimaryEmailAddress = "user@example.com",
                EmployeeType = "Regular",
                Status = "Active",
                Id = "Test123"
            }));

            _controller = new EmployeesController(_query, _logger);
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
        }
    }
}
