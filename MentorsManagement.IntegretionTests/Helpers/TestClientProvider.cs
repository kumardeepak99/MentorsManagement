using System.Text;
using Newtonsoft.Json;

namespace MentorsManagement.IntegrationTests.Helpers
{
    public class TestClientProvider
    {
        public static StringContent GetJsonHttpContent(object items)
        {
            return new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json");
        }

        internal static class Urls
        {
            public readonly static string GetAllMentors = "/Mentors/GetAllMentorsAsync";
            public readonly static string GetMentorById = "/Mentors/GetMentorByIdAsync/";
            public readonly static string CreateMentor = "/Mentors/CreateMentorAsync";
            public readonly static string UpdateMentor = "/Mentors/UpdateMentorAsync";
            public readonly static string DeleteMentor = "/Mentors/DeleteMentorAsync/";


        }

    }
}
