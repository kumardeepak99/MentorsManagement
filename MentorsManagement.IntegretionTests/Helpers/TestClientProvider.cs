using System.Text;
using Newtonsoft.Json;

namespace MentorsManagement.IntegrationTests.Helpers
{
    public class TestClientProvider
    {
        //const string BaseUrlAddress = "https://localhost:44355";
        //public HttpClient _client { get; private set; }

        //public TestClientProvider()
        //{
        //    var server = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
        //    _client = server.CreateClient();
        //    _client.BaseAddress = new Uri(BaseUrlAddress);
        //}

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
