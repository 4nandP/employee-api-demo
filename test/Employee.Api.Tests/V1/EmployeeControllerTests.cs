using FluentAssertions;
using FluentAssertions.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
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
        public async Task GetDetails_ShouldReturnMatchingRecord_WhenIdIsValid(string id)
        {
            await AssertMatchingRecordReturned(id).ConfigureAwait(false);
        }

        [Theory]
        [InlineData("Noone")]
        [InlineData("Nobody")]
        public async Task GetDetails_ShouldReturnNotFound_WhenIdIsInvalid(string id)
        {
            // Arrange
            var url = $"/api/v1/Employee/GetDetails/{id}";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetDetails_ShouldReturnBadRequest_WhenIdNotProvided()
        {
            // Arrange
            var url = $"/api/v1/Employee/GetDetails/";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetDetails_ShouldReturnMatchingSchema_WhenIdIsTest123()
        {
            // Arrange
            var expectedResponse = await ResourceHelper.GetJsonResource("Employee.Api.Tests.V1.Test123.json").ConfigureAwait(false);

            // Act
            var jsonContent = await AssertMatchingRecordReturned("Test123").ConfigureAwait(false);

            // Assert
            jsonContent.Should().BeEquivalentTo(expectedResponse);
        }

        private async Task<JToken> AssertMatchingRecordReturned(string id)
        {
            // Arrange
            var url = $"/api/v1/Employee/GetDetails/{id}";
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url).ConfigureAwait(false);

            // Assert
            response.IsSuccessStatusCode.Should().Be(true);
            response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
            var contentRaw = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            var content = JToken.Parse(contentRaw);
            content.Should().HaveElement("QueryResponse");
            content.SelectToken("QueryResponse").Should().HaveElement("Employee");
            content.SelectToken("QueryResponse").SelectToken("Employee").Should().HaveElement("Id");
            content.SelectToken("QueryResponse").SelectToken("Employee").SelectToken("Id").Should().HaveValue(id);

            return content;
        }
    }
}
