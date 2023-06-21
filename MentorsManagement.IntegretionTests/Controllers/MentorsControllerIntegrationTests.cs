using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit;
using MentorsManagement.API.Models;
using MentorsManagement.IntegrationTests.Helpers;
using MentorsManagement.IntegretionTests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using MentorsManagement.API.DbContexts;

namespace MentorsManagement.IntegretionTests.Controllers
{

    public class MentorsControllerIntegrationTests
      : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _applicationFactory;
        private readonly HttpClient _client;
        private readonly Fixture _fixture;
        private readonly MentorDbContext _dbContext;

        public MentorsControllerIntegrationTests()
        {
            _applicationFactory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.RemoveAll(typeof(DbContext));
                    });
                });

            _client = _applicationFactory.CreateClient();
            _fixture = new Fixture();

            using var scope = _applicationFactory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<MentorDbContext>();
            TestingDbConfiguration.InitializeDbForTests(_dbContext);
        }

        public async Task<IEnumerable<Mentor>> GetAllMentorsFromDb()
        {
            var response = await _client.GetAsync(TestClientProvider.Urls.GetAllMentors);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var mentors = await response.Content.ReadFromJsonAsync<IEnumerable<Mentor>>();

            return mentors!=null ? mentors : new List<Mentor>(); ;
        }


        [Fact]
        public async Task GetAllMentors_ListOfMentors_ReturnsListOfMentors()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(TestClientProvider.Urls.GetAllMentors);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var mentors = await response.Content.ReadFromJsonAsync<IEnumerable<Mentor>>();

            mentors.Should().NotBeNull();

            mentors?.Count().Should().BeGreaterThanOrEqualTo(1);

        }

        [Fact]
        public async Task<Mentor?> GetMentorById_WithExistingMentor_ReturnsMentor()
        {
            // Arrange
            var mentorId = 1; // Provide a valid mentor ID for an existing mentor in db mentor table

            // Act
            var response = await _client.GetAsync(TestClientProvider.Urls.GetMentorById+mentorId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var mentor = await response.Content.ReadFromJsonAsync<Mentor>();

            mentor.Should().NotBeNull();
            mentor?.MentorId.Should().Be(mentorId);
            return mentor;
        }

        [Fact]
        public async Task GetMentorById_WithNonExistingMentor_ReturnsNotFoundStatus()
        {
            // Arrange
            var mentorId = 9999; // Provide a non-existing mentor ID

            // Act
            var response = await _client.GetAsync(TestClientProvider.Urls.GetMentorById + mentorId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateMentor_CreatesMentorWithMentorData_ReturnsCreatedMentor()
        {
            // Arrange

            var mentors = await GetAllMentorsFromDb();
            int lastMentorId = mentors==null ? 0 : mentors.Last().MentorId;
            lastMentorId++;
            var newMentor = _fixture.Create<Mentor>();
            newMentor.MentorId=lastMentorId;

            // Act
            var createdMentorResponse = await _client.PostAsJsonAsync(TestClientProvider.Urls.CreateMentor, newMentor);

            // Assert
            createdMentorResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var createdMentor = await createdMentorResponse.Content.ReadFromJsonAsync<Mentor>();

            createdMentor.Should().NotBeNull();
            createdMentor.Should().BeEquivalentTo(newMentor);

            // add code for deleting newly created Mentor if req
        }

        [Fact]
        public async Task CreateMentor_WithDuplicateMentorId_ReturnsConflict()
        {
            // Arrange
            var existingMentorId = 1;
            var newMentor = _fixture.Create<Mentor>();
            newMentor.MentorId = existingMentorId;  // Set the ID to an existing mentor's ID

            // Act
            var response = await _client.PostAsJsonAsync(TestClientProvider.Urls.CreateMentor, newMentor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }

        [Fact]
        public async Task CreateMentor_WithInvalidMentorData_ReturnsInternalServerError()
        {
            // Arrange
            var newMentor = new Fixture().Create<Mentor>();
            newMentor.MentorId=-1;    // making PK to 0                

            // Act
            var response = await _client.PostAsJsonAsync(TestClientProvider.Urls.CreateMentor, newMentor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task UpdateMentor_UpdatesGivenMentorWithData_ReturnsUpdatedMentor()
        {
            // Arrange
            var mentorId = 1; // Provide a valid mentor ID for an existing mentor in db mentor table
            var updatedMentor = _fixture.Create<Mentor>();
            updatedMentor.MentorId=mentorId;
            // Act
            var response = await _client.PutAsJsonAsync(TestClientProvider.Urls.UpdateMentor, updatedMentor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var updatedMentorResponse = await response.Content.ReadFromJsonAsync<Mentor>();

            updatedMentorResponse.Should().NotBeNull();
            updatedMentorResponse.Should().BeEquivalentTo(updatedMentor);

        }

        [Fact]
        public async Task UpdateMentor_WhenMentorDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var mentorId = 9999; // Provide a non-existing mentor ID
            var updatedMentor = _fixture.Create<Mentor>();
            updatedMentor.MentorId=mentorId;

            // Act
            var response = await _client.PutAsJsonAsync(TestClientProvider.Urls.UpdateMentor, updatedMentor);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteMentor_DeletesMentorWithId_ReturnsNoContent()
        {
            // Arrange
            var mentorId = 4; // Provide a valid mentor ID for an existing mentor in the system

            // Act
            var response = await _client.DeleteAsync(TestClientProvider.Urls.DeleteMentor+mentorId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task DeleteMentor_WhenMentorIdDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            var mentorId = 9999; // Provide a non-existing mentor ID

            // Act
            var response = await _client.DeleteAsync(TestClientProvider.Urls.DeleteMentor + mentorId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}
