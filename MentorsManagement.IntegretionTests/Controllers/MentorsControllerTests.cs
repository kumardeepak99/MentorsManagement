using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MentorsManagement.API.Models;
using MentorsManagement.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Xunit;

namespace MentorsManagement.IntegretionTests.Controllers
{

    public class MentorsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly HttpClient _client;
        //WebApplicationFactory<Program> factory
        public MentorsControllerTests()
        {
            _applicationFactory = new WebApplicationFactory<Program>();
            _client=_applicationFactory.CreateClient();
        }

        [Fact]
        public async Task GetAllMentors_ReturnsListOfMentors()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(TestClientProvider.Urls.GetAllMentors);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var mentors = await response.Content.ReadFromJsonAsync<IEnumerable<Mentor>>();

            mentors.Should().NotBeNull();
        }
    }
}
