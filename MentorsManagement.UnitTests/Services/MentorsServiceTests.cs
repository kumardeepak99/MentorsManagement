using AutoFixture;
using FluentAssertions;
using MentorsManagement.API.DBContexts;
using MentorsManagement.API.Models;
using MentorsManagement.API.Services;
using MentorsManagement.UnitTests.Helpers;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;


namespace MentorsManagement.UnitTests.Services
{
    public class MentorsServiceTests
    {
        private readonly MentorService _mentorService;
        private readonly Mock<IMentorDbContext> _mockDbContext;
        private readonly Fixture _fixture;

        public MentorsServiceTests()
        {
            _mockDbContext = new Mock<IMentorDbContext>();
            _fixture = new Fixture();
            _mentorService = new MentorService(_mockDbContext.Object);
        }

        [Fact]
        public async Task GetAllMentors_ReturnsListOfMentors()
        {
            // Arrange
            var mentors = _fixture.CreateMany<Mentor>(2).ToList();
            var mockDbSet = CreateMockDbSet(mentors);
            _mockDbContext.Setup(c => c.Mentors).Returns(mockDbSet.Object);

            // Act
            var result = await _mentorService.GetAllMentors();

            // Assert
            result.Should().BeEquivalentTo(mentors);
        }

        [Fact]
        public async Task GetAllMentors_ReturnsEmptyList_WhenNoMentorsExist()
        {
            // Arrange
            var mockDbSet = CreateMockDbSet(new List<Mentor>());
            _mockDbContext.Setup(c => c.Mentors).Returns(mockDbSet.Object);

            // Act
            var result = await _mentorService.GetAllMentors();

            // Assert
            result.Should().BeEmpty();
        }


        private static Mock<DbSet<TEntity>> CreateMockDbSet<TEntity>(List<TEntity> data) where TEntity : class
        {
            var mockDbSet = new Mock<DbSet<TEntity>>();
            var queryableData = data.AsQueryable();

            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Provider).Returns(queryableData.Provider);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.Expression).Returns(queryableData.Expression);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.ElementType).Returns(queryableData.ElementType);
            mockDbSet.As<IQueryable<TEntity>>().Setup(m => m.GetEnumerator()).Returns(queryableData.GetEnumerator());
            mockDbSet.As<IAsyncEnumerable<TEntity>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new InMemoryAsyncEnumerator<TEntity>(queryableData.GetEnumerator()));

            return mockDbSet;
        }

    }
}
