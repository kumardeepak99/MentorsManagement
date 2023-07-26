using MentorsManagement.API.Models;

namespace MentorsManagement.API.Services
{
    public interface IMentorService
    {
        Task<List<Mentor>> GetAllMentorsAsync();
        Task<Mentor?> GetMentorByIdAsync(string id);
        Task<Mentor?> CreateMentorAsync(Mentor mentor);
        Task<Mentor?> UpdateMentorAsync(Mentor mentor);
        void DeleteMentorAsync(string id);
    }
}
