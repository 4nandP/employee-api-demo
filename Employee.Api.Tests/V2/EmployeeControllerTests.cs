using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Employee.Api.Tests.V2
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
            var url = $"/api/v2/Employee/{id}/Details";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Contains(id, content);
        }

        [Theory]
        [InlineData("Noone")]
        [InlineData("Nobody")]
        public async Task GetDetails_EndpointsReturnNotFound(string id)
        {
            // Arrange
            var url = $"/api/v2/Employee/{id}/Details";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetDetails_EndpointsReturnMatchingSchema()
        {
            // Arrange
            var url = $"/api/v2/Employee/Test123/Details";
            var client = _factory.CreateClient();
            var expectedResponse = await ResourceHelper.GetJsonResource("Employee.Api.Tests.V2.Test123.json").ConfigureAwait(false);

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);
            var content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var jsonContent = JObject.Parse(content);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            Assert.Equal(expectedResponse, jsonContent);
        }
    }
}
