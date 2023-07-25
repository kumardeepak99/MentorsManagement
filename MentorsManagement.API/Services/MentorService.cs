using MentorsManagement.API.Data;
using MentorsManagement.API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MentorsManagement.API.Services
{
    public class MentorService : IMentorService
    {
        private readonly IMongoCollection<Mentor> _mentors;
        public MentorService(IMongoDatabase mongoDatabase, IOptions<MongoDbSettings> mongoDbSettings)
        {
            _mentors = mongoDatabase.GetCollection<Mentor>(mongoDbSettings.Value.MentorsCollectionName);
        }

        public async Task<List<Mentor>> GetAllMentors()
        {
            var allMentors = await _mentors.Find(_ => true).ToListAsync();
            return allMentors;
        }
        public async Task<Mentor?> GetMentorById(string id)
        {
            var mentor = await _mentors.Find(m => m.Id == id).FirstOrDefaultAsync();
            return mentor;
        }

        public async Task<Mentor?> CreateMentor(Mentor mentor)
        {
            if (mentor.Id != string.Empty)
            {
                return null;
            }

            await _mentors.InsertOneAsync(mentor);
            return mentor;
        }

        public async Task<Mentor?> UpdateMentor(Mentor mentor)
        {
            var existingMentor = await _mentors.Find(m => m.Id == mentor.Id).FirstOrDefaultAsync();
            if (existingMentor == null)
                return null;

            existingMentor.FirstName = mentor.FirstName;
            existingMentor.LastName = mentor.LastName;
            existingMentor.BirthDay = mentor.BirthDay;
            existingMentor.Address = mentor.Address;

            await _mentors.ReplaceOneAsync(m => m.Id == mentor.Id, existingMentor);
            return existingMentor;
        }

        public async Task<bool> DeleteMentor(string id)
        {
            var deleteResult = await _mentors.DeleteOneAsync(m => m.Id == id);
            return deleteResult.DeletedCount > 0;
        }
    }
}
