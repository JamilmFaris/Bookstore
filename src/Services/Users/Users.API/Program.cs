using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Users.Infrastructure.Data;
using Users.Domain.Interfaces;
using Users.Infrastructure.Repositories;
using Users.Application;
using Users.Application.Commands;
using Users.Application.Mappings;
using Users.Application.Common;
var builder = WebApplication.CreateBuilder(args);
var env = builder.Configuration["Environment"];
builder.Environment.EnvironmentName = env ?? "Development";
// Add DbContext
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Configure JWT authentication
// Configure JWT authentication - UPDATED VERSION
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSettings["Secret"];

if (string.IsNullOrEmpty(secret))
    throw new Exception("JWT Secret is missing in configuration");

if (secret.Length < 32)
    throw new Exception("JWT Secret must be at least 32 characters");

var key = Encoding.UTF8.GetBytes(secret); // Changed from ASCII to UTF-8

builder.Services.Configure<JwtSettings>(jwtSettings);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    
    x.Events = new JwtBearerEvents
    {
        OnTokenValidated = async context =>
        {
            var token = context.SecurityToken?.ToString(); 
            var repository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            
            if (!string.IsNullOrEmpty(token) && 
                await repository.IsTokenBlacklistedAsync(token))
            {
                context.Fail("Token has been invalidated");
            }
        }
    };
});

// Add MediatR and AutoMapper
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(RegisterUserCommand).Assembly));
builder.Services.AddAutoMapper(typeof(UserProfile).Assembly);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    // Apply migrations in development
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        db.Database.Migrate();
    }
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

