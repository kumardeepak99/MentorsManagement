namespace MentorsManagement.API.Data
{
    public class MongoDbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string MentorsCollectionName { get; set; }
    }
}
