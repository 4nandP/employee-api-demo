using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Employee.Api.Tests.V2
{
    public class EmployeeControllerTests : IClassFixture<WebApplicationFactory<Employee.Api.Startup>>
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
        public async Task GetDetails_ShouldReturnMatchingRecord_WhenIdIsValid(string id)
        {
            await AssertMatchingRecordReturned(id);
        }

        [Theory]
        [InlineData("Noone")]
        [InlineData("Nobody")]
        public async Task GetDetails_ShouldReturnNotFound_WhenIdIsInvalid(string id)
        {
            // Arrange
            var url = $"/api/v2/Employees/{id}/Detail";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetDetails_ShouldReturnMatchingSchema_WhenIdIsTest123()
        {
            // Arrange
            var expectedResponse = await ResourceHelper.GetJsonResource("Employee.Api.Tests.V2.Test123.json");

            // Act
            var jsonContent = await AssertMatchingRecordReturned("Test123");

            // Assert
            jsonContent.Should().BeEquivalentTo(expectedResponse);
        }

        private async Task<JToken> AssertMatchingRecordReturned(string id)
        {
            // Arrange
            var url = $"/api/v2/Employees/{id}/Detail";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.IsSuccessStatusCode.Should().Be(true);
            var contentRaw = await response.Content.ReadAsStringAsync();
            var content = JToken.Parse(contentRaw);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
            content.Should().HaveElement("id");
            content.SelectToken("id").Should().HaveValue(id);

            return content;
        }
    }
}
