using AutoFixture;
using FluentAssertions;
using MentorsManagement.API.Data;
using MentorsManagement.API.Models;
using MentorsManagement.API.Services;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Xunit;

namespace MentorsManagement.UnitTests.Services
{
    public class MentorServiceTests : IDisposable
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly Fixture _fixture;

        public MentorServiceTests()
        {
            var databaseName = Guid.NewGuid().ToString();
            var mongoClient = new MongoClient();
            _mongoDatabase = mongoClient.GetDatabase(databaseName);

            _fixture = new Fixture();
            _fixture.Customize<Mentor>(c => c.Without(m => m.Id));
        }

        public void Dispose()
        {
            _mongoDatabase.Client.DropDatabase(_mongoDatabase.DatabaseNamespace.DatabaseName);
        }

        private MongoDbSettings GetMongoDbSettings() => new MongoDbSettings
        {
            ConnectionString = "mongodb+srv://deepakkumarjha:MongoDb2023@cluster0.wgqjg0t.mongodb.net/?retryWrites=true&w=majority",
            DatabaseName = "MentorsDb",
            MentorsCollectionName = "mentors",
        };

        [Fact]
        public async Task GetAllMentors_ReturnsListOfMentors()
        {
            // Arrange
            var mentorCollection = _mongoDatabase.GetCollection<Mentor>("mentors");
            var mentors = _fixture.CreateMany<Mentor>(2).ToList();

            // Inserting the test data into the MongoDB collection
            await mentorCollection.InsertManyAsync(mentors);

            // Create an instance of MentorService with db configuration
            var mentorService = new MentorService(_mongoDatabase, Options.Create(GetMongoDbSettings()));

            // Act
            var result = await mentorService.GetAllMentorsAsync();

            // Assert
            result.Should().BeEquivalentTo(mentors, options => options.Excluding(x => x.BirthDay));
        }

        [Fact]
        public async Task GetAllMentors_ReturnsEmptyList_WhenNoMentorsExist()
        {
            // Arrange
            var mentorService = new MentorService(_mongoDatabase, Options.Create(GetMongoDbSettings()));

            // Act
            var result = await mentorService.GetAllMentorsAsync();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetMentorById_WhenMentorExist_ReturnsMentorFromDatabase()
        {
            // Arrange
            var mentorCollection = _mongoDatabase.GetCollection<Mentor>("mentors");
            var mentor = _fixture.Create<Mentor>();
            await mentorCollection.InsertOneAsync(mentor);

            var mentorService = new MentorService(_mongoDatabase, Options.Create(GetMongoDbSettings()));

            // Act
            var result = await mentorService.GetMentorByIdAsync(mentor.Id);

            // Assert
            result.Should().BeEquivalentTo(mentor, options => options.Excluding(x => x.BirthDay));

        }

        [Fact]
        public async Task GetMentorById_WhenMentorDoesNotExist_ReturnsNull()
        {
            // Arrange
            var mentorId = ObjectId.GenerateNewId().ToString();
            var mentorService = new MentorService(_mongoDatabase, Options.Create(GetMongoDbSettings()));

            // Act
            var result = await mentorService.GetMentorByIdAsync(mentorId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task CreateMentor_AddsMentorToDatabase_ReturnsCreatedMentor()
        {
            // Arrange
            var mentorCollection = _mongoDatabase.GetCollection<Mentor>("mentors");
            var mentor = _fixture.Create<Mentor>();
            mentor.Id = string.Empty;
            var mentorService = new MentorService(_mongoDatabase, Options.Create(GetMongoDbSettings()));

            // Act
            var createdMentor = await mentorService.CreateMentorAsync(mentor);

            // Assert
            var result = await mentorCollection.Find(x => x.Id == createdMentor.Id).FirstOrDefaultAsync();
            result.Should().BeEquivalentTo(mentor, options => options.Excluding(x => x.BirthDay));

        }

        [Fact]
        public async Task UpdateMentor_UpdatesMentorInDatabase_ReturnsUpdatedMentor()
        {
            // Arrange
            var mentorCollection = _mongoDatabase.GetCollection<Mentor>("mentors");
            var mentor = _fixture.Create<Mentor>();
            await mentorCollection.InsertOneAsync(mentor);

            var mentorService = new MentorService(_mongoDatabase, Options.Create(GetMongoDbSettings()));

            var updatedMentor = _fixture.Create<Mentor>();
            updatedMentor.Id = mentor.Id;

            // Act
            await mentorService.UpdateMentorAsync(updatedMentor);

            // Assert
            var result = await mentorCollection.Find(x => x.Id == updatedMentor.Id).FirstOrDefaultAsync();
            result.Should().BeEquivalentTo(updatedMentor, options => options.Excluding(x => x.BirthDay));

        }

        [Fact]
        public async Task UpdateMentor_WhenMentorDoesNotExist_ReturnsException()
        {
            // Arrange
            var mentorCollection = _mongoDatabase.GetCollection<Mentor>("mentors");
            var mentor = _fixture.Create<Mentor>();
            await mentorCollection.InsertOneAsync(mentor);

            var mentorService = new MentorService(_mongoDatabase, Options.Create(GetMongoDbSettings()));

            var updatedMentor = _fixture.Create<Mentor>();
            updatedMentor.Id = ObjectId.GenerateNewId().ToString();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(async () => await mentorService.UpdateMentorAsync(updatedMentor));
        }

        [Fact]
        public async Task DeleteMentor_RemovesMentorFromDatabase()
        {
            // Arrange
            var mentorCollection = _mongoDatabase.GetCollection<Mentor>("mentors");
            var mentor = _fixture.Create<Mentor>();
            await mentorCollection.InsertOneAsync(mentor);

            var mentorService = new MentorService(_mongoDatabase, Options.Create(GetMongoDbSettings()));

            // Act
            mentorService.DeleteMentorAsync(mentor.Id);

            // Assert
            var dbMentor = await mentorCollection.Find(x => x.Id == mentor.Id).FirstOrDefaultAsync();
            dbMentor.Should().BeNull();
        }

    }
}


