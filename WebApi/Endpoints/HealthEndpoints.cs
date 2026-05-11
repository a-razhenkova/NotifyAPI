using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebApi
{
    public static class HealthEndpoints
    {
        public static WebApplication MapHealthEndpoints(this WebApplication app)
        {
            app.MapGet("api/health/heartbeat", () =>
            {
                return Results.Ok();
            });

            app.MapGet("api/health", (IHostEnvironment environment) =>
            {
                var response = new DeployInfoResponse()
                {
                    Version = WebApiAssembly.GetVersion(),
                    Environment = environment.EnvironmentName,
                    MachineName = Environment.MachineName,
                    MachineTimestamp = DateTime.Now
                };

                return Results.Ok(response);
            });

            app.MapGet("api/health/checks", async (HealthCheckService healthChecker) =>
            {
                HealthReport response = await healthChecker.CheckHealthAsync();
                return Results.Ok(response);
            });

            return app;
        }
    }
}