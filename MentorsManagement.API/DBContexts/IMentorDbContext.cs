using MentorsManagement.API.Models;
using Microsoft.EntityFrameworkCore;

namespace MentorsManagement.API.DBContexts
{
    public interface IMentorDbContext
    {
        DbSet<Mentor> Mentors { get; set; }
    }
}
