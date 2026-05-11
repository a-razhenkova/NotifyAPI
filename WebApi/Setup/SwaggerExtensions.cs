using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi
{
    public static class SwaggerExtensions
    {
        public static WebApplication UseSwagger(this WebApplication app)
        {
            if (app.Environment.IsSwaggerAllowed())
            {
                app.UseSwagger(cfg =>
                {
                    cfg.RouteTemplate = "api/{documentName}/swagger.json";
                });

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/api/v1/swagger.json", "v1");
                    c.RoutePrefix = "api";
                });
            }

            return app;
        }

        public static WebApplicationBuilder AddSwagger(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsSwaggerAllowed())
                builder.Services.AddSwaggerGen(opt => opt.AddSummaryAndContact());

            return builder;
        }

        private static bool IsSwaggerAllowed(this IWebHostEnvironment environment)
        {
            return environment.IsDevelopment() || environment.IsStaging();
        }

        private static void AddSummaryAndContact(this SwaggerGenOptions opt)
        {
            opt.SwaggerDoc("v1", new OpenApiInfo()
            {
                Title = "Notify API",
                Description = @"<p>A minimal API that consumes messages and sends notifications.</p>",
                Version = $"{WebApiAssembly.GetVersion()}",
                Contact = new OpenApiContact()
                {
                    Name = "Aleksandrina Razhenkova",
                    Url = new Uri("https://github.com/a-razhenkova"),
                    Email = "a.razhenkova@gmail.com"
                }
            });
        }
    }
}