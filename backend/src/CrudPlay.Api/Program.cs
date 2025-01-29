using CrudPlay.Application.Queries;
using CrudPlay.Infrastructure;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// services
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(GetTodosQuery).Assembly));

builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();
PersistanceInjection.InitializeDatabase(app.Services);

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();

    // Swagger UI
    //app.UseSwaggerUI(options => 
    //{ 
    //    options.SwaggerEndpoint("/openapi/v1.json", "CrudPlay API V1");
    //});

    // Scalar
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("CrudPlay API V1")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
