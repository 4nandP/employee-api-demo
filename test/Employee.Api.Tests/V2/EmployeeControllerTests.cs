using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using Xunit;
using FluentAssertions;
using FluentAssertions.Json;


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
        public async Task GetDetails_EndpointsReturnMatchingRecord(string id)
        {
            await AssertMatchingRecordReturned(id);
        }

        private async Task<JToken> AssertMatchingRecordReturned(string id)
        {
            // Arrange
            var url = $"/api/v2/Employee/{id}/Details";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);
            var contentRaw = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var content = JToken.Parse(contentRaw);


            // Assert
            response.IsSuccessStatusCode.Should().Be(true);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
            content.Should().HaveElement("id");
            content.SelectToken("id").Should().HaveValue(id);

            return content;
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
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetDetails_EndpointsReturnMatchingSchema()
        {
            // Arrange
            var expectedResponse = await ResourceHelper.GetJsonResource("Employee.Api.Tests.V2.Test123.json").ConfigureAwait(false);

            // Act
            var jsonContent = await AssertMatchingRecordReturned("Test123");

            // Assert
            jsonContent.Should().BeEquivalentTo(expectedResponse);
        }
    }
}
