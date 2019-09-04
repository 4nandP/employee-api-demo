using Employee.Api.V1.Application.Queries;
using Employee.Api.V1.Controllers;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Employees.Tests.V1.Controllers
{
    public class EmployeeControllerTests
    {
        private IEmployeeQueries _query;
        private EmployeeController _controller;

        [SetUp]
        public void Setup()
        {
            _query = A.Fake<IEmployeeQueries>();

            A.CallTo(() => _query.FindByIdAsync(A<string>.Ignored, A<CancellationToken>.Ignored)).Returns(Task.FromResult<EmployeeQueryResponse>(null));
            A.CallTo(() => _query.FindByIdAsync("Test123", A<CancellationToken>.Ignored)).Returns(Task.FromResult(new EmployeeQueryResponse(new Employee.Api.V1.Domain.Employee
            {
                Organization = false,
                Title = "Mrs.",
                GivenName = "Jane",
                MiddleName = "Lane",
                FamilyName = "Doe",
                DisplayName = "Jane Lane Doe",
                PrintOnCheckName = "Jane Lane Doe",
                Active = true,
                PrimaryPhone = new Employee.Api.V1.Domain.Phone { FreeFormNumber = "505.555.9999" },
                PrimaryEmailAddress = new Employee.Api.V1.Domain.EmailAddress { Address = "user@example.com" },
                EmployeeType = "Regular",
                Status = "Active",
                Id = "Test123"
            })));

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
        }
    }
}
