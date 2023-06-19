using System.Net;
using System.Net.Http.Json;
using AutoFixture;
using FluentAssertions;
using MentorsManagement.API.Models;
using MentorsManagement.IntegrationTests.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
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
        public async Task<IEnumerable<Mentor>> GetAllMentors_ReturnsListOfMentors()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync(TestClientProvider.Urls.GetAllMentors);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var mentors = await response.Content.ReadFromJsonAsync<IEnumerable<Mentor>>();

            mentors.Should().NotBeNull();

            mentors?.Count().Should().BeGreaterThanOrEqualTo(1);
            return mentors;
        }

        [Fact]
        public async Task<Mentor?> GetMentorById_ReturnsMentor()
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
        public async Task CreateMentor_ReturnsCreatedMentor()
        {
            // Arrange

            //var response = await _client.GetAsync(TestClientProvider.Urls.GetAllMentors);
            //response.StatusCode.Should().Be(HttpStatusCode.OK);
            //var mentors = await response.Content.ReadFromJsonAsync<IEnumerable<Mentor>>();
            //mentors.Should().NotBeNull();

            var mentors = await GetAllMentors_ReturnsListOfMentors();
            mentors.Should().NotBeNull();
            int lastMentorId = mentors==null ? 0 : mentors.Last().MentorId;
            lastMentorId++;
            var newMentor = new Fixture().Create<Mentor>();
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
        public async Task UpdateMentor_ReturnsUpdatedMentor()
        {
            // Arrange
            var mentorId = 20; // Provide a valid mentor ID for an existing mentor in db mentor table
            var updatedMentor = new Fixture().Create<Mentor>();
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
        public async Task DeleteMentor_ReturnsNoContent()
        {
            // Arrange
            var mentorId = 21; // Provide a valid mentor ID for an existing mentor in the system

            // Act
            var response = await _client.DeleteAsync(TestClientProvider.Urls.DeleteMentor+mentorId);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
