using System.Text;

using Asp.Versioning;

using CrudPlay.Api.Helpers;
using CrudPlay.Api.Middleware;
using CrudPlay.Application;
using CrudPlay.Core;
using CrudPlay.Core.Identity;
using CrudPlay.Core.Options;
using CrudPlay.Infrastructure;
using CrudPlay.Infrastructure.Persistance.Identity;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

AddVersioning(builder.Services);
builder.Services.AddControllers();

builder.Services.AddCore();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplication();

AddAuthenticationWithJwtBearer(builder.Services, builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

var app = builder.Build();

PersistanceInjection.InitializeDatabase(app.Services);
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.MapOpenApi();
    app.MapScalarApiReference(options =>
    {
        options
        .WithTitle("CrudPlay API")
        .WithHttpBearerAuthentication(bearer =>
        {
            bearer.Token = "bear-my-token";
        })
        .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app
    .MapGroup("/api/auth")
    .MapIdentityApi<ApplicationUser>()
    .WithTags("Authentication")
    .WithOpenApi();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();

void AddVersioning(IServiceCollection services)
{
    services
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
}

void AddAuthenticationWithJwtBearer(IServiceCollection services, IConfiguration configuration)
{
    var jwtOptions = configuration.GetSection("JwtConfiguration").Get<JwtOptions>();

    services
        .AddIdentityApiEndpoints<ApplicationUser>(options =>
        {


            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.SignIn.RequireConfirmedAccount = false;
            options.User.RequireUniqueEmail = true;

            options.ClaimsIdentity.UserIdClaimType = JwtRegisteredClaimNames.Sub;
        })
        .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
        .AddDefaultTokenProviders();

    services
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                ClockSkew = TimeSpan.Zero
            };

            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine($"JWT Authentication Failed: {context.Exception.Message}");
                    return Task.CompletedTask;
                }
            };
        });

    services.AddAuthorization();
}