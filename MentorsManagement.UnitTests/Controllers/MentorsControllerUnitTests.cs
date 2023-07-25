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
    public class MentorsControllerUnitTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IMentorService> _mockMentorsService;

        public MentorsControllerUnitTests()
        {
            _fixture = new Fixture();
            _mockMentorsService = new Mock<IMentorService>();
        }

        private MentorsController CreateMentorsController() => new MentorsController(_mockMentorsService.Object);

        [Fact]
        public async Task GetAllMentors_OnSuccess_ReturnsOkStatusCode()
        {
            // Arrange
            var mentors = _fixture.Create<List<Mentor>>();
            _mockMentorsService.Setup(service => service.GetAllMentors()).ReturnsAsync(mentors);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.GetAllMentorsAsync();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(mentors);
        }

        [Fact]
        public async Task GetAllMentors_OnSuccess_InvokesMentorService()
        {
            // Arrange
            var mentors = _fixture.Create<List<Mentor>>();
            _mockMentorsService.Setup(service => service.GetAllMentors()).ReturnsAsync(mentors);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.GetAllMentorsAsync();

            // Assert
            _mockMentorsService.Verify(service => service.GetAllMentors(), Times.Once());
        }

        [Fact]
        public async Task GetAllMentors_OnSuccess_ReturnsListOfMentors()
        {
            // Arrange
            var mentors = _fixture.Create<List<Mentor>>();
            _mockMentorsService.Setup(service => service.GetAllMentors()).ReturnsAsync(mentors);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.GetAllMentorsAsync();

            // Assert
            result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeOfType<List<Mentor>>();
        }

        [Fact]
        public async Task GetAllMentors_OnNoMentorsFound_ReturnsNotFoundStatusCode()
        {
            // Arrange
            _mockMentorsService.Setup(service => service.GetAllMentors()).ReturnsAsync(new List<Mentor>());
            var sut = CreateMentorsController();

            // Act
            var result = await sut.GetAllMentorsAsync();

            // Assert
            var notFoundResult = result.Should().BeOfType<NotFoundResult>().Subject;
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetAllMentors_OnFailure_ReturnsInternalServerError()
        {
            // Arrange
            _mockMentorsService.Setup(service => service.GetAllMentors()).ThrowsAsync(new Exception());
            var sut = CreateMentorsController();

            // Act
            var result = await sut.GetAllMentorsAsync();

            // Assert
            var statusResult = result.Should().BeOfType<StatusCodeResult>().Subject;
            statusResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task GetMentorById_OnSuccess_ReturnsMentorWithOkStatusCode()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService.Setup(service => service.GetMentorById(mentor.Id)).ReturnsAsync(mentor);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.GetMentorByIdAsync(mentor.Id);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnedMentor = okResult.Value.Should().BeAssignableTo<Mentor>().Subject;
            returnedMentor.Should().BeEquivalentTo(mentor);
        }

        [Fact]
        public async Task GetMentorById_OnMentorNotFound_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var mentorId = _fixture.Create<string>();
            _mockMentorsService.Setup(service => service.GetMentorById(mentorId)).ReturnsAsync((Mentor)null);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.GetMentorByIdAsync(mentorId);

            // Assert
            var notFoundResult = result.Should().BeOfType<NotFoundResult>().Subject;
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetMentorById_OnFailure_ReturnsInternalServerError()
        {
            // Arrange
            var mentorId = _fixture.Create<string>();
            _mockMentorsService.Setup(service => service.GetMentorById(mentorId)).ThrowsAsync(new Exception());
            var sut = CreateMentorsController();

            // Act
            var result = await sut.GetMentorByIdAsync(mentorId);

            // Assert
            var statusResult = result.Should().BeOfType<StatusCodeResult>().Subject;
            statusResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task CreateMentor_OnSuccess_ReturnsCreatedMentorWithOkStatusCode()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            mentor.Id = string.Empty;
            _mockMentorsService.Setup(service => service.CreateMentor(It.IsAny<Mentor>())).ReturnsAsync(mentor);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.CreateMentorAsync(mentor);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var createdMentor = okResult.Value.Should().BeAssignableTo<Mentor>().Subject;
            createdMentor.Should().BeEquivalentTo(mentor);
        }


        [Fact]
        public async Task CreateMentor_OnSuccess_InvokesMentorService()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            mentor.Id = string.Empty;
            _mockMentorsService.Setup(service => service.CreateMentor(It.IsAny<Mentor>())).ReturnsAsync(mentor);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.CreateMentorAsync(mentor);

            // Assert
            _mockMentorsService.Verify(service => service.CreateMentor(It.IsAny<Mentor>()), Times.Once());
        }

        [Fact]
        public async Task CreateMentor_OnFailure_ReturnsInternalServerError()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService.Setup(service => service.CreateMentor(It.IsAny<Mentor>())).ThrowsAsync(new Exception());
            var sut = CreateMentorsController();

            // Act
            var result = await sut.CreateMentorAsync(mentor);

            // Assert
            var statusResult = result.Should().BeOfType<StatusCodeResult>().Subject;
            statusResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task UpdateMentor_OnSuccess_ReturnsUpdatedMentorWithOkStatusCode()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService.Setup(service => service.UpdateMentor(mentor)).ReturnsAsync(mentor);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.UpdateMentorAsync(mentor);

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var updatedMentor = okResult.Value.Should().BeAssignableTo<Mentor>().Subject;
            updatedMentor.Should().BeEquivalentTo(mentor);
        }


        [Fact]
        public async Task UpdateMentor_OnSuccess_InvokesMentorService()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService.Setup(service => service.UpdateMentor(mentor)).ReturnsAsync(mentor);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.UpdateMentorAsync(mentor);

            // Assert
            _mockMentorsService.Verify(service => service.UpdateMentor(mentor), Times.Once());
        }

        [Fact]
        public async Task UpdateMentor_WhenMentorDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService.Setup(service => service.UpdateMentor(mentor)).ReturnsAsync((Mentor?)null);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.UpdateMentorAsync(mentor);

            // Assert
            var notFoundResult = result.Should().BeOfType<NotFoundResult>().Subject;
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task UpdateMentor_OnFailure_ReturnsInternalServerError()
        {
            // Arrange
            var mentor = _fixture.Create<Mentor>();
            _mockMentorsService.Setup(service => service.UpdateMentor(mentor)).ThrowsAsync(new Exception());
            var sut = CreateMentorsController();

            // Act
            var result = await sut.UpdateMentorAsync(mentor);

            // Assert
            var statusResult = result.Should().BeOfType<StatusCodeResult>().Subject;
            statusResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }

        [Fact]
        public async Task DeleteMentor_OnSuccess_ReturnsNoContentStatusCode()
        {
            // Arrange
            var mentorId = _fixture.Create<string>();
            _mockMentorsService.Setup(service => service.DeleteMentor(mentorId)).ReturnsAsync(true);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.DeleteMentorAsync(mentorId);

            // Assert
            var noContentResult = result.Should().BeOfType<NoContentResult>().Subject;
            noContentResult.StatusCode.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task DeleteMentor_OnSuccess_InvokesMentorService()
        {
            // Arrange
            var mentorId = _fixture.Create<string>();
            _mockMentorsService.Setup(service => service.DeleteMentor(mentorId)).ReturnsAsync(true);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.DeleteMentorAsync(mentorId);

            // Assert
            _mockMentorsService.Verify(service => service.DeleteMentor(mentorId), Times.Once());
        }


        [Fact]
        public async Task DeleteMentor_WhenMentorDoesNotExist_ReturnsNotFoundStatusCode()
        {
            // Arrange
            var mentorId = _fixture.Create<string>();
            _mockMentorsService.Setup(service => service.DeleteMentor(mentorId)).ReturnsAsync(false);
            var sut = CreateMentorsController();

            // Act
            var result = await sut.DeleteMentorAsync(mentorId);

            // Assert
            var notFoundResult = result.Should().BeOfType<NotFoundResult>().Subject;
            notFoundResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task DeleteMentor_OnFailure_ReturnsInternalServerError()
        {
            // Arrange
            var mentorId = _fixture.Create<string>();
            _mockMentorsService.Setup(service => service.DeleteMentor(mentorId)).ThrowsAsync(new Exception());
            var sut = CreateMentorsController();

            // Act
            var result = await sut.DeleteMentorAsync(mentorId);

            // Assert
            var statusResult = result.Should().BeOfType<StatusCodeResult>().Subject;
            statusResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
