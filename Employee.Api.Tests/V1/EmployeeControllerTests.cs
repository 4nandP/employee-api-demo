using Microsoft.AspNetCore.Mvc.Testing;
using System.Threading.Tasks;
using Xunit;

namespace Employee.Api.Tests.V1
{
    public class EmployeeControllerTests
        : IClassFixture<WebApplicationFactory<Employee.Api.Startup>>
    {
        private readonly WebApplicationFactory<Employee.Api.Startup> _factory;

        public EmployeeControllerTests(WebApplicationFactory<Employee.Api.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("Test123")]
        [InlineData("Test456")]
        [InlineData("Test789")]
        public async Task GetDetails_EndpointsReturnMatchingRecord(string id)
        {
            // Arrange
            var url = $"/api/v1/Employee/GetDetails/{id}";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
        }

        [Theory]
        [InlineData("Noone")]
        [InlineData("Nobody")]
        public async Task GetDetails_EndpointsReturnNotFound(string id)
        {
            // Arrange
            var url = $"/api/v1/Employee/GetDetails/{id}";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetDetails_EndpointsReturnBadRequest()
        {
            // Arrange
            var url = $"/api/v1/Employee/GetDetails/";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
