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
            _mockMentorsService = new Mock<IMentorService>();
        }

        [Fact]
        public async Task GetAllMentors_OnSuccess_ReturnsOkStatusCode()
        {
            // Arrange
            var mentors = _fixture.Create<List<Mentor>>();
            _mockMentorsService
                .Setup(service => service.GetAllMentors())
                .ReturnsAsync(mentors);

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.GetAllMentorsAsync();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status200OK);
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
            var result = await sut.GetAllMentorsAsync();

            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.Value.Should().BeOfType<List<Mentor>>();
        }

        [Fact]
        public async Task GetAllMentors_OnNoMentorsFound_ReturnsNotFoundStatusCode()
        {
            // Arrange
            _mockMentorsService
                .Setup(service => service.GetAllMentors())
                .ReturnsAsync(new List<Mentor>());

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.GetAllMentorsAsync();

            // Assert
            result.Should().BeOfType<NotFoundResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
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

        [Fact]
        public async Task GetMentorById_OnSuccess_ReturnsMentorWithOkStatusCode()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService
                .Setup(service => service.GetMentorById(mentor.MentorId))
                .ReturnsAsync(mentor);

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.GetMentorByIdAsync(mentor.MentorId);

            // Assert
            var returnedMentor = result.Should().BeOfType<OkObjectResult>().Subject.Value as Mentor;

            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            returnedMentor.Should().NotBeNull();
            returnedMentor.Should().BeEquivalentTo(mentor);
        }

        [Fact]
        public async Task GetMentorById_OnMentorNotFound_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var mentorId = _fixture.Create<int>();
            _mockMentorsService
                .Setup(service => service.GetMentorById(mentorId))
                .ReturnsAsync((Mentor)null); // Return null when mentor does not exist

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.GetMentorByIdAsync(mentorId);

            // Assert
            result.Should().BeOfType<NotFoundResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetMentorById_OnFailure_ReturnsInternalServerError()
        {
            // Arrange
            var mentorId = _fixture.Create<int>();
            _mockMentorsService
                .Setup(service => service.GetMentorById(mentorId))
                .ThrowsAsync(new Exception());

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.GetMentorByIdAsync(mentorId);

            // Assert
            result.Should().BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task CreateMentor_OnSuccess_ReturnsCreatedMentorWithOkStatusCode()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService
                .Setup(service => service.CreateMentor(It.IsAny<Mentor>()))
                .ReturnsAsync(mentor);

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.CreateMentorAsync(mentor);
            var createdMentor = result.Should().BeOfType<OkObjectResult>().Subject.Value as Mentor;
            // Assert
            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            createdMentor.Should().NotBeNull();
            createdMentor.Should().BeEquivalentTo(mentor);
        }

        [Fact]
        public async Task CreateMentor_OnSuccess_InvokesMentorService()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService
                .Setup(service => service.CreateMentor(It.IsAny<Mentor>()))
                .ReturnsAsync(mentor);

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.CreateMentorAsync(mentor);

            // Assert
            _mockMentorsService.Verify(
                service => service.CreateMentor(It.IsAny<Mentor>()),
                Times.Once());
        }

        [Fact]
        public async Task CreateMentor_OnFailure_ReturnsInternalServerError()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService
                .Setup(service => service.CreateMentor(It.IsAny<Mentor>()))
                .ThrowsAsync(new Exception());

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.CreateMentorAsync(mentor);

            // Assert
            result.Should().BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task UpdateMentor_OnSuccess_ReturnsUpdatedMentorWithOkStatusCode()
        {
            // Arrange
            var mentorId = _fixture.Create<int>();
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService
                .Setup(service => service.UpdateMentor(mentor))
                .ReturnsAsync(mentor);

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.UpdateMentorAsync(mentor);

            // Assert
            var updatedMentor = result.Should().BeOfType<OkObjectResult>().Subject.Value as Mentor;

            result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status200OK);

            updatedMentor.Should().NotBeNull();
            updatedMentor.Should().BeEquivalentTo(mentor);

        }

        [Fact]
        public async Task UpdateMentor_OnSuccess_InvokesMentorService()
        {
            // Arrange
            var mentorId = _fixture.Create<int>();
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService
                .Setup(service => service.UpdateMentor(mentor))
                .ReturnsAsync(mentor);

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.UpdateMentorAsync(mentor);

            // Assert
            _mockMentorsService.Verify(
                service => service.UpdateMentor(mentor),
                Times.Once());
        }

        [Fact]
        public async Task UpdateMentor_WhenMentorDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var mentorId = _fixture.Create<int>();
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService
                .Setup(service => service.UpdateMentor(mentor))
                .ReturnsAsync((Mentor?)null); // Return null when mentor does not exist

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            mentor.MentorId = mentorId;
            var result = await sut.UpdateMentorAsync(mentor);

            // Assert
            result.Should().BeOfType<NotFoundResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task UpdateMentor_OnFailure_ReturnsInternalServerError()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService
                .Setup(service => service.UpdateMentor(mentor))
                .ThrowsAsync(new Exception());

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.UpdateMentorAsync(mentor);

            // Assert
            result.Should().BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task DeleteMentor_OnSuccess_ReturnsNoContentStatusCode()
        {
            // Arrange
            var mentorId = _fixture.Create<int>();
            _mockMentorsService
                .Setup(service => service.DeleteMentor(mentorId))
                .ReturnsAsync(true);

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.DeleteMentorAsync(mentorId);

            // Assert
            result.Should().BeOfType<NoContentResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task DeleteMentor_OnSuccess_InvokesMentorService()
        {
            // Arrange
            var mentorId = _fixture.Create<int>();
            _mockMentorsService
                .Setup(service => service.DeleteMentor(mentorId))
                .ReturnsAsync(true);

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.DeleteMentorAsync(mentorId);

            // Assert
            _mockMentorsService.Verify(
                service => service.DeleteMentor(mentorId),
                Times.Once());
        }

        [Fact]
        public async Task DeleteMentor_WhenMentorDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var mentorId = _fixture.Create<int>();
            _mockMentorsService
                .Setup(service => service.DeleteMentor(mentorId))
                .ReturnsAsync(false);

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.DeleteMentorAsync(mentorId);

            // Assert
            result.Should().BeOfType<NotFoundResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task DeleteMentor_OnFailure_ReturnsInternalServerError()
        {
            // Arrange
            var mentorId = _fixture.Create<int>();
            _mockMentorsService
                .Setup(service => service.DeleteMentor(mentorId))
                .ThrowsAsync(new Exception());

            var sut = new MentorsController(_mockMentorsService.Object);

            // Act
            var result = await sut.DeleteMentorAsync(mentorId);

            // Assert
            result.Should().BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }

}