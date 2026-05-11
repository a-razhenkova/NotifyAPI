using System.Text.Json;
using System.Text.Json.Serialization;
using WebApi;

var builder = WebApplication.CreateBuilder(args);
builder.AddLogger();

builder.AddServices();
builder.AddHealthChecks();

builder.Services.ConfigureHttpJsonOptions(opt =>
{
    opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseUpper));
});

await builder.AddRabbitMqAsync();

builder.AddSwagger();

var app = builder.Build();
app.UseHttpsRedirection();

app.MapHealthEndpoints();
app.UseSwagger();

await app.StartConsumeRabbitMqMessages();

await app.RunAsync();