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

        public async Task<List<Mentor>> GetAllMentorsAsync()
        {
            return await _mentors.Find(_ => true).ToListAsync();
        }
        public async Task<Mentor?> GetMentorByIdAsync(string id)
        {
            return await _mentors.Find(m => m.Id == id).FirstOrDefaultAsync();
        }

        public async Task<Mentor?> CreateMentorAsync(Mentor mentor)
        {
            if (!string.IsNullOrEmpty(mentor.Id))
            {
                throw new ArgumentException("Mentor ID should be null or empty.");
            }
            await _mentors.InsertOneAsync(mentor);
            return mentor;
        }

        public async Task<Mentor?> UpdateMentorAsync(Mentor mentor)
        {
            var existingMentor = await _mentors.Find(m => m.Id == mentor.Id).FirstOrDefaultAsync();
            if (existingMentor == null)
            {
                throw new Exception("Mentor not found.");
            }
            await _mentors.ReplaceOneAsync(m => m.Id == mentor.Id, mentor);
            return mentor;
        }

        public async void DeleteMentorAsync(string id)
        {
            await _mentors.DeleteOneAsync(m => m.Id == id);

        }
    }
}
