using System.ComponentModel.DataAnnotations;

namespace MentorsManagement.API.Models
{
    public class Mentor
    {
        [Key]
        public int MentorId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public string Address { get; set; }

    }
}
