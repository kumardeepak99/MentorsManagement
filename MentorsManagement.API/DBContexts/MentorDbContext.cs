using MentorsManagement.API.DBContexts;
using MentorsManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace StudentManagement.API.DbContexts
{
    public class MentorDbContext : DbContext, IMentorDbContext
    {
        public MentorDbContext(DbContextOptions<MentorDbContext> options) : base(options)
        {
        }

        public MentorDbContext() : base()
        {
        }

        public DbSet<Mentor> Mentors { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Mentor>()
                .HasKey(m => m.MentorId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
