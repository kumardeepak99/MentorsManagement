using MentorsManagement.API.Models;

namespace MentorsManagement.API.Services
{
    public interface IMentorService
    {
        Task<List<Mentor>> GetAllMentors();
        Task<Mentor?> GetMentorById(int id);
        Task<Mentor> CreateMentor(Mentor mentor);
        Task<Mentor?> UpdateMentor(Mentor mentor);
        Task<bool> DeleteMentor(int id);
    }
}
