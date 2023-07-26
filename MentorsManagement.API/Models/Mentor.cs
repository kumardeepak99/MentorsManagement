using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MentorsManagement.API.Models
{
    [BsonIgnoreExtraElements]
    public class Mentor
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDay { get; set; }
        public string City { get; set; }

    }
}
