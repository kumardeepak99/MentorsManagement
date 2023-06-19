using MentorsManagement.API.DBContexts;
using MentorsManagement.API.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Mentor?> GetMentorById(int id)
        {
            return await _context.Mentors.FindAsync(id);
        }

        public async Task<Mentor?> CreateMentor(Mentor mentor)
        {
            var isMentorExisting= await _context.Mentors.FindAsync(mentor.MentorId);
            if (isMentorExisting != null)
            {
                return null;
            }
            _context.Mentors.Add(mentor);
            await _context.SaveChangesAsync();
            return mentor;
        }

        public async Task<Mentor?> UpdateMentor(Mentor mentor)
        {
            var existingMentor = await _context.Mentors.FindAsync(mentor.MentorId);
            if (existingMentor == null)
                return null;
            existingMentor.FirstName=mentor.FirstName;
            existingMentor.LastName=mentor.LastName;
            existingMentor.BirthDay=mentor.BirthDay;
            existingMentor.Address=mentor.Address;
            await _context.SaveChangesAsync();
            return existingMentor;
        }

        public async Task<bool> DeleteMentor(int id)
        {
            var mentor = await _context.Mentors.FindAsync(id);
            if (mentor == null)
                return false;

            _context.Mentors.Remove(mentor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
