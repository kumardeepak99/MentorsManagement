using MentorsManagement.API.DBContexts;
using MentorsManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using StudentManagement.API.DbContexts;

namespace MentorsManagement.API.Services
{
    public class MentorService : IMentorService
    {
        private readonly IMentorDbContext _context;

        public MentorService(IMentorDbContext context)
        {
            _context = context;
        }

        public async Task<List<Mentor>> GetAllMentors()
        {
            return await _context.Mentors.ToListAsync();
        }
    }
}
