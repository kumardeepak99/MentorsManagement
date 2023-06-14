using MentorsManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.API.DbContexts
{
    public class MentorDbContext : DbContext
    {
        public MentorDbContext(DbContextOptions<MentorDbContext> options) : base(options)
        {
        }

        public DbSet<Mentor> Mentors { get; set; }
    }
}
