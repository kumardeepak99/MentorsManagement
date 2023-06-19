using AutoFixture;
using FluentAssertions;
using MentorsManagement.API.Models;
using MentorsManagement.API.Services;
using Microsoft.EntityFrameworkCore;
using StudentManagement.API.DbContexts;
using Xunit;


namespace MentorsManagement.UnitTests.Services
{

    public class MentorServiceTests
    {
        private readonly DbContextOptions<MentorDbContext> _dbContextOptions;
        private readonly Fixture _fixture;

        public MentorServiceTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MentorDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _fixture = new Fixture();
        }

        private MentorDbContext CreateDbContext()
        {
            return new MentorDbContext(_dbContextOptions);
        }

        [Fact]
        public async Task GetAllMentors_ReturnsListOfMentors()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                var mentors = _fixture.CreateMany<Mentor>(2).ToList();
                dbContext.Mentors.AddRange(mentors);
                dbContext.SaveChanges();

                var mentorService = new MentorService(dbContext);

                // Act
                var result = await mentorService.GetAllMentors();

                // Assert
                result.Should().BeEquivalentTo(mentors);
            }
        }

        [Fact]
        public async Task GetAllMentors_ReturnsEmptyList_WhenNoMentorsExist()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                var mentorService = new MentorService(dbContext);

                // Act
                var result = await mentorService.GetAllMentors();

                // Assert
                result.Should().BeEmpty();
            }
        }

        [Fact]
        public async Task CreateMentor_AddsMentorToDatabase_ReturnsCreatedMentor()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                var mentor = _fixture.Create<Mentor>();
                var mentorService = new MentorService(dbContext);

                // Act
                var cretatedMentor = await mentorService.CreateMentor(mentor);

                // Assert
                dbContext.Mentors.Should().Contain(mentor);
                cretatedMentor.Should().BeEquivalentTo(mentor);
            }
        }

        [Fact]
        public async Task GetMentorById_ReturnsMentorFromDatabase()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                var mentor = _fixture.Create<Mentor>();
                dbContext.Mentors.Add(mentor);
                dbContext.SaveChanges();

                var mentorService = new MentorService(dbContext);

                // Act
                var result = await mentorService.GetMentorById(mentor.MentorId);

                // Assert
                result.Should().BeEquivalentTo(mentor);
            }
        }

        [Fact]
        public async Task UpdateMentor_UpdatesMentorInDatabase_ReturnsUpdatedMentor()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                var mentor = _fixture.Create<Mentor>();
                dbContext.Mentors.Add(mentor);
                dbContext.SaveChanges();

                var mentorService = new MentorService(dbContext);
                var updatedMentor = _fixture.Create<Mentor>();
                updatedMentor.MentorId = mentor.MentorId;

                // Act
                await mentorService.UpdateMentor(updatedMentor);

                // Assert
                var result = dbContext.Mentors.Find(mentor.MentorId);
                result.Should().BeEquivalentTo(updatedMentor);
            }
        }

        [Fact]
        public async Task DeleteMentor_RemovesMentorFromDatabase()
        {
            // Arrange
            using (var dbContext = CreateDbContext())
            {
                var mentor = _fixture.Create<Mentor>();
                dbContext.Mentors.Add(mentor);
                dbContext.SaveChanges();

                var mentorService = new MentorService(dbContext);

                // Act
                await mentorService.DeleteMentor(mentor.MentorId);

                // Assert
                dbContext.Mentors.Should().NotContain(mentor);
            }
        }
    }
}
