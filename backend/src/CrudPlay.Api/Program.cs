using Asp.Versioning;

using CrudPlay.Api.Middleware;
using CrudPlay.Application;
using CrudPlay.Core;
using CrudPlay.Infrastructure;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Versioning
builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = ApiVersionReader.Combine(
            new UrlSegmentApiVersionReader(),
            new HeaderApiVersionReader("X-Api-Version")
        );
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });

// Services
builder.Services.AddCore();
builder.Services.AddApplication();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

var app = builder.Build();

PersistanceInjection.InitializeDatabase(app.Services);
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    ///app.UseDeveloperExceptionPage();
    app.MapOpenApi();

    // Swagger UI
    //app.UseSwaggerUI(options => 
    //{ 
    //    options.SwaggerEndpoint("/openapi/v1.json", "CrudPlay API");
    //});

    // Scalar
    app.MapScalarApiReference(options =>
    {
        options
            .WithTitle("CrudPlay API")
            .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();
