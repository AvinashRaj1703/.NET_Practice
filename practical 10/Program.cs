using Practical_10.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Register controller services
builder.Services.AddControllers();

// Register Swagger services with metadata
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Practical 10 - Student Management API",
        Version = "v1",
        Description = "ASP.NET Core Web API for Student Management with CRUD operations and request logging middleware."
    });
});

var app = builder.Build();

// Enable Swagger and custom styled Swagger UI
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Practical 10 API v1");
    c.DocumentTitle = "Practical 10 - Student Management API";
    c.InjectStylesheet("/css/swagger-custom.css");
});

// Enable static web assets (serves modern Student Portal at root /)
app.UseDefaultFiles();
app.UseStaticFiles();

// Register custom request logging middleware
app.UseMiddleware<RequestLoggingMiddleware>();

// Enable authorization middleware
app.UseAuthorization();

// Map controller routes
app.MapControllers();

// Start application
app.Run();
