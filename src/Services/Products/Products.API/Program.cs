using Microsoft.EntityFrameworkCore;
using Products.Application;
using Products.Infrastructure.Data;
using Products.Infrastructure.Repositories;
using Products.Domain.Interfaces;
using Products.Application.Queries;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<BookstoreContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorCodesToAdd: null);
        });

    // Enable detailed errors in development
    if (builder.Environment.IsDevelopment())
    {
        options.EnableDetailedErrors();
        options.EnableSensitiveDataLogging();
    }
});

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(GetBooksListQuery).Assembly));

// Register repository
builder.Services.AddScoped<IBookRepository, BookRepository>();

// Register application services
builder.Services.AddApplicationServices();

// Add controllers and API explorer
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = null; // Preserve case
    });

builder.Services.AddEndpointsApiExplorer();

// Configure Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Products API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Products API v1");
    });

    // // Apply pending migrations in development
    // using (var scope = app.Services.CreateScope())
    // {
    //     var db = scope.ServiceProvider.GetRequiredService<BookstoreContext>();
    //     db.Database.Migrate();
    // }
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Add health check endpoint
app.MapGet("/health", async (BookstoreContext db) =>
    Results.Ok(new
    {
        dbStatus = await db.Database.CanConnectAsync(),
        migrationsApplied = (await db.Database.GetAppliedMigrationsAsync()).Count()
    }));

app.Run();