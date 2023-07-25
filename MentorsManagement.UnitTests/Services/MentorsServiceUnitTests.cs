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
            _fixture.Customize<Mentor>(c => c.Without(m => m.Id)); // Exclude the Id
        }

        // Drop the in-memory database after tests
        public void Dispose()
        {
            _mongoDatabase.Client.DropDatabase(_mongoDatabase.DatabaseNamespace.DatabaseName);
        }

        [Fact]
        public async Task GetAllMentors_ReturnsListOfMentors()
        {
            // Arrange
            var mentorCollection = _mongoDatabase.GetCollection<Mentor>("mentors");
            var mentors = _fixture.CreateMany<Mentor>(2).ToList();

            // Insert the test data into the MongoDB collection
            await mentorCollection.InsertManyAsync(mentors);

            // Create an instance of MongoDbSettings with the desired configuration
            var mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb+srv://deepakkumarjha:MongoDb2023@cluster0.wgqjg0t.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "MentorsDb",
                MentorsCollectionName = "mentors",
            };

            var mentorService = new MentorService(_mongoDatabase, Options.Create(mongoDbSettings));

            // Act
            var result = await mentorService.GetAllMentors();

            // Assert
            result.Should().BeEquivalentTo(mentors, options => options
                .Excluding(x => x.BirthDay)); // Exclude BirthDay property from comparison

        }


        [Fact]
        public async Task GetAllMentors_ReturnsEmptyList_WhenNoMentorsExist()
        {
            // Arrange
            var mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb+srv://deepakkumarjha:MongoDb2023@cluster0.wgqjg0t.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "MentorsDb",
                MentorsCollectionName = "mentors",
            };

            var mentorService = new MentorService(_mongoDatabase, Options.Create(mongoDbSettings));

            // Act
            var result = await mentorService.GetAllMentors();

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

            var mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb+srv://deepakkumarjha:MongoDb2023@cluster0.wgqjg0t.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "MentorsDb",
                MentorsCollectionName = "mentors",
            };

            var mentorService = new MentorService(_mongoDatabase, Options.Create(mongoDbSettings));

            // Act
            var result = await mentorService.GetMentorById(mentor.Id);

            // Assert
            result.Should().BeEquivalentTo(mentor, options => options
                .Excluding(x => x.BirthDay));
        }

        [Fact]
        public async Task GetMentorById_WhenMentorDoesNotExist_ReturnsNull()
        {
            // Arrange
            var mentorId = ObjectId.GenerateNewId().ToString();
            var mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb+srv://deepakkumarjha:MongoDb2023@cluster0.wgqjg0t.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "MentorsDb",
                MentorsCollectionName = "mentors",
            };

            var mentorService = new MentorService(_mongoDatabase, Options.Create(mongoDbSettings));

            // Act
            var result = await mentorService.GetMentorById(mentorId);

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
            var mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb+srv://deepakkumarjha:MongoDb2023@cluster0.wgqjg0t.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "MentorsDb",
                MentorsCollectionName = "mentors",
            };

            var mentorService = new MentorService(_mongoDatabase, Options.Create(mongoDbSettings));

            // Act
            var createdMentor = await mentorService.CreateMentor(mentor);

            // Assert
            var dbMentor = await mentorCollection.Find(x => x.Id == createdMentor.Id).FirstOrDefaultAsync();
            dbMentor.Should().BeEquivalentTo(mentor, options => options
                .Excluding(x => x.BirthDay)); // Exclude BirthDay property from comparison
        }

        [Fact]
        public async Task UpdateMentor_UpdatesMentorInDatabase_ReturnsUpdatedMentor()
        {
            // Arrange
            var mentorCollection = _mongoDatabase.GetCollection<Mentor>("mentors");
            var mentor = _fixture.Create<Mentor>();
            await mentorCollection.InsertOneAsync(mentor);

            var mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb+srv://deepakkumarjha:MongoDb2023@cluster0.wgqjg0t.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "MentorsDb",
                MentorsCollectionName = "mentors",
            };

            var mentorService = new MentorService(_mongoDatabase, Options.Create(mongoDbSettings));

            var updatedMentor = _fixture.Create<Mentor>();
            updatedMentor.Id = mentor.Id;

            // Act
            await mentorService.UpdateMentor(updatedMentor);

            // Assert
            var dbMentor = await mentorCollection.Find(x => x.Id == updatedMentor.Id).FirstOrDefaultAsync();
            dbMentor.Should().BeEquivalentTo(updatedMentor, options => options
                .Excluding(x => x.BirthDay));
        }

        [Fact]
        public async Task UpdateMentor_WhenMentorDoesNotExist_ReturnsNull()
        {
            // Arrange
            var mentorCollection = _mongoDatabase.GetCollection<Mentor>("mentors");
            var mentor = _fixture.Create<Mentor>();
            await mentorCollection.InsertOneAsync(mentor);

            var mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb+srv://deepakkumarjha:MongoDb2023@cluster0.wgqjg0t.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "MentorsDb",
                MentorsCollectionName = "mentors",
            };

            var mentorService = new MentorService(_mongoDatabase, Options.Create(mongoDbSettings));

            var updatedMentor = _fixture.Create<Mentor>();
            updatedMentor.Id = ObjectId.GenerateNewId().ToString();
            // Act
            var result = await mentorService.UpdateMentor(updatedMentor);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteMentor_RemovesMentorFromDatabase()
        {
            // Arrange
            var mentorCollection = _mongoDatabase.GetCollection<Mentor>("mentors");
            var mentor = _fixture.Create<Mentor>();
            await mentorCollection.InsertOneAsync(mentor);

            var mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb+srv://deepakkumarjha:MongoDb2023@cluster0.wgqjg0t.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "MentorsDb",
                MentorsCollectionName = "mentors",
            };

            var mentorService = new MentorService(_mongoDatabase, Options.Create(mongoDbSettings));

            // Act
            await mentorService.DeleteMentor(mentor.Id);

            // Assert
            var dbMentor = await mentorCollection.Find(x => x.Id == mentor.Id).FirstOrDefaultAsync();
            dbMentor.Should().BeNull();
        }

        [Fact]
        public async Task DeleteMentor_WhenMentorDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var mongoDbSettings = new MongoDbSettings
            {
                ConnectionString = "mongodb+srv://deepakkumarjha:MongoDb2023@cluster0.wgqjg0t.mongodb.net/?retryWrites=true&w=majority",
                DatabaseName = "MentorsDb",
                MentorsCollectionName = "mentors",
            };

            var mentorService = new MentorService(_mongoDatabase, Options.Create(mongoDbSettings));

            var mentorId = ObjectId.GenerateNewId().ToString();

            // Act
            var result = await mentorService.DeleteMentor(mentorId);

            // Assert
            result.Should().BeFalse();
        }

    }
}
