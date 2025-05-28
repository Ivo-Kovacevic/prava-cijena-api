using System.Text;
using CloudinaryDotNet;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PravaCijena.Api.Config;
using PravaCijena.Api.Database;
using PravaCijena.Api.Middlewares;
using PravaCijena.Api.Models;

Env.Load();
var builder = WebApplication.CreateBuilder(args);
var isProduction = builder.Environment.IsProduction();
var configuration = builder.Configuration;
builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.WriteIndented = true; });

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var jwtSigningKeyFromConfig = configuration["JWT:SigningKey"];
var jwtIssuerFromConfig = configuration["JWT:Issuer"];
var jwtAudienceFromConfig = configuration["JWT:Audience"];
var databaseConnection = configuration["ConnectionStrings:DefaultConnection"];

// Connect to database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(databaseConnection).EnableSensitiveDataLogging());

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = true;
        options.Password.RequiredLength = 6;
    })
    .AddEntityFrameworkStores<AppDbContext>();

if (string.IsNullOrEmpty(jwtSigningKeyFromConfig))
{
    throw new InvalidOperationException("JWT:SigningKey not found in configuration.");
}

var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSigningKeyFromConfig));
builder.Services.AddSingleton(symmetricSecurityKey);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuerFromConfig,
        ValidateAudience = true,
        ValidAudience = jwtAudienceFromConfig,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = symmetricSecurityKey,
        ValidateLifetime = true
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["jwtToken"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddAppServices();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddCors(options =>
{
    options.AddPolicy("_myAllowSpecificOrigins",
        policy =>
        {
            policy
                .WithOrigins(isProduction ? "https://www.pravacijena.eu" : "http://localhost:3000")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options => options.EnableTryItOutByDefault());
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("_myAllowSpecificOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler();
app.MapControllers();

// Add Postgre Trigrams extension that provides searching for products functionality
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.ExecuteSqlRaw("CREATE EXTENSION IF NOT EXISTS pg_trgm;");
}

app.MapGet("/",
    () => Results.Ok(
        "PravaCijena API is live. Check available endpoints at 'https://github.com/Ivo-Kovacevic/prava-cijena-api'"));
app.MapFallback(() => Results.Problem(title: "Endpoint not found", statusCode: StatusCodes.Status404NotFound));

// Cloudinary
var cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
cloudinary.Api.Secure = true;

app.Run();