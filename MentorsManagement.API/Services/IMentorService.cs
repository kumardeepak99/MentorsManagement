using MentorsManagement.API.Models;

namespace MentorsManagement.API.Services
{
    public interface IMentorService
    {
        Task<List<Mentor>> GetAllMentors();
    }
}
