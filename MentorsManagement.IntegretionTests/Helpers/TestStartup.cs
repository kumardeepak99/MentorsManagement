using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MentorsManagement.IntegrationTests.Helpers
{
    public class TestStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            // Add any other required services for your API
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                app.UseHttpsRedirection();
            });
        }

    }
}
