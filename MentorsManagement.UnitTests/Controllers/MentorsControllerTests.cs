using AutoFixture;
using FluentAssertions;
using MentorsManagement.API.Controllers;
using MentorsManagement.API.Models;
using MentorsManagement.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace MentorsManagement.UnitTests.Controllers
{
    public class MentorsControllerTests
    {
        private Fixture _fixture;
        private Mock<IMentorService> _mockMentorsService;

        public MentorsControllerTests()
        {
            _fixture = new Fixture();
            _mockMentorsService=new Mock<IMentorService>();
        }

        [Fact]
        public async Task GetAllMentors_OnSuccess_ReturnsStatusCode200()
        {
            // Arrange
            var mentors = _fixture.Create<List<Mentor>>();
            _mockMentorsService.
                Setup(service => service.GetAllMentors())
                .ReturnsAsync(mentors);

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = (OkObjectResult)await sut.GetAllMentorsAsync();

            // Assert
            result.StatusCode.Should().Be(200);
        }

        [Fact]
        public async Task GetAllMentors_OnSuccess_InvokesMentorService()
        {
            // Arrange
            _mockMentorsService
                .Setup(service => service.GetAllMentors())
                .ReturnsAsync(new List<Mentor>());

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.GetAllMentorsAsync();

            // Assert

            _mockMentorsService.Verify(
                service => service.GetAllMentors(),
                Times.Once());

        }

        [Fact]
        public async Task GetAllMentors_OnSuccess_ReturnsListOfMentors()
        {
            // Arrange
            var mentors = _fixture.Create<List<Mentor>>();
            _mockMentorsService
                .Setup(service => service.GetAllMentors())
                .ReturnsAsync(mentors);

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = (OkObjectResult)await sut.GetAllMentorsAsync();

            // Assert

            result.Should().BeOfType<OkObjectResult>();
            var objectResult = (ObjectResult)result;

            objectResult.Value.Should().BeOfType<List<Mentor>>();

        }

        [Fact]
        public async Task GetAllMentors_OnNoMentorsFound_ReturnsStatusCode404()
        {
            // Arrange
            _mockMentorsService
                .Setup(service => service.GetAllMentors())
                .ReturnsAsync(new List<Mentor>());

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.GetAllMentorsAsync();

            // Assert

            result.Should().BeOfType<NotFoundResult>();
            var objectResult = (NotFoundResult)result;
            objectResult.StatusCode.Should().Be(404);
        }

        [Fact]
        public async Task GetAllMentors_OnFailure_ReturnsInternalServerError()
        {
            // Arrange
            _mockMentorsService
                .Setup(service => service.GetAllMentors())
                .ThrowsAsync(new Exception());

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.GetAllMentorsAsync();

            // Assert
            result.Should().BeOfType<StatusCodeResult>()
                  .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

    }
}
