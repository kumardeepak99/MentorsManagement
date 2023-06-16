using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;

namespace MentorsManagement.IntegrationTests.Helpers
{
    public class TestClientProvider
    {
        public HttpClient _client { get; private set; }

        public TestClientProvider()
        {
            var server = new TestServer(new WebHostBuilder().UseStartup<TestStartup>());
            _client = server.CreateClient();
            _client.BaseAddress = new Uri("https://localhost:44355");
        }

        public static StringContent GetJsonHttpContent(object items)
        {
            return new StringContent(JsonConvert.SerializeObject(items), Encoding.UTF8, "application/json");
        }

        internal static class Urls
        {
            //public readonly static string BaseUrlAddress = "https://localhost:44355";
            public readonly static string GetAllMentors = "/Mentors/GetAllAsync";

        }

    }
}
