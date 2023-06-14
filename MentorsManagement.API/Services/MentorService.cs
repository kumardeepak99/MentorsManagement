using MentorsManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using StudentManagement.API.DbContexts;

namespace MentorsManagement.API.Services
{
    public class MentorService : IMentorService
    {
        private readonly MentorDbContext _context;

        public MentorService(MentorDbContext context)
        {
            _context = context;
        }

        public async Task<List<Mentor>> GetAllMentors()
        {
            return await _context.Mentors.ToListAsync();
        }
    }
}
