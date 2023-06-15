using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MentorsManagement.API.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace MentorsManagement.IntegretionTests.Controllers
{

    public class MentorsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        public MentorsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllMentors_ReturnsListOfMentors()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/Mentors/GetAllAsync");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var mentors = await response.Content.ReadFromJsonAsync<IEnumerable<Mentor>>();

            mentors.Should().NotBeNull();
        }
    }
}
