using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotifyEnhancer.Config;
using SpotifyEnhancer.DataAccess;
using SpotifyEnhancer.DataAccess.Interfaces;

namespace SpotifyEnhancer
{
     public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        #region snippet_ConfigureServices
        public void ConfigureServices(IServiceCollection services)
        {
            // Config, DB, and swagger
            services.AddSingleton(Configuration)
                .AddOpenApiDocument(config =>
                {
                    config.PostProcess = document =>
                    {
                        document.Info.Version = "v1";
                        document.Info.Title = "Spotify enhancer API";
                        document.Info.Description = "An API service to enhance various aspects of your spotify library";
                        document.Info.TermsOfService = "None";
                        document.Info.Contact = new NSwag.OpenApiContact
                        {
                            Name = "Brendan Giberson",
                            Email = "bcgiberson@gmail.com",
                            Url = "https://www.linkedin.com/in/brendangiberson/"
                        };
                        document.Info.License = new NSwag.OpenApiLicense
                        {
                            Name = "Use under LICX",
                            Url = "https://example.com/license"
                        };
                    };
                })
                .Configure<AppleMusicConfig>(config =>
                {
                    config.DeveloperKey = Configuration.GetValue<string>("Apple:DeveloperKey");
                })
                .Configure<SpotifyConfig>(config =>
                {
                    config.Token = Configuration.GetValue<string>("Spotify:Token");
                })
                .AddTransient<IAppleMusicHttpClient, AppleMusicHttpClient>()
                .AddTransient<HttpClient>()
                .AddHttpClient()
                .AddControllers();
        }
        #endregion

        #region snippet_Configure
        public void Configure(IApplicationBuilder app)
        {
            app.UseStaticFiles();

            app.UseOpenApi();
            app.UseSwaggerUi3();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        #endregion
    }
}