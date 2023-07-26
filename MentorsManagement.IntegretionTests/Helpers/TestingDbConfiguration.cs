using MentorsManagement.API.DbContexts;
using MentorsManagement.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MentorsManagement.IntegretionTests.Helpers
{
    public static class TestingDbConfiguration
    {
        public static IConfiguration GetConfiguration() =>
          new ConfigurationBuilder()
         .SetBasePath(Directory.GetCurrentDirectory())
         .AddJsonFile("appsettings.testing.json", true, true)
         .Build();

        public static MentorDbContext GetContext(IConfiguration configuration)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MentorDbContext>();
            optionsBuilder.UseSqlServer("Data Source=CESIT-LAP-0451;Initial Catalog=MENTORS_MGMT_TEST;User ID=sa;Password=Welcome@#123; trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;");
            return new MentorDbContext(optionsBuilder.Options);
        }

        public static List<Mentor> GetSeedingMentors()
        {
            return new List<Mentor>()
            {
                new Mentor() { Id = "1" ,FirstName = "Deepak", LastName = "Kumar", BirthDay = new DateTime(1999-05-15), City="45-uppal Hyd"},
                new Mentor() { Id = "2", FirstName = "Naveen", LastName = "Kumar", BirthDay = new DateTime(2002-06-25), City="8-Hitech City Hyd"},
                new Mentor() { Id = "3", FirstName = "Aashik", LastName = "Villa", BirthDay = new DateTime(2001-04-14), City="7-Maple Rd AP"},
                new Mentor() { Id = "4", FirstName = "Kuldeep",LastName = "Chauhan", BirthDay = new DateTime(2001-07-10),City="7-St Indore"},

            };
        }

        public static void InitializeDbForTests(MentorDbContext db)
        {
            db.Mentors.RemoveRange(db.Mentors);
            var mentors = GetSeedingMentors();
            db.Mentors.AddRange(mentors);
            db.SaveChanges();
        }
    }
}
