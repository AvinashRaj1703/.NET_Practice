
using Microsoft.EntityFrameworkCore;
using PlacementDashboard.Data;

var builder = WebApplication.CreateBuilder(args);

// Register Razor Pages
builder.Services.AddRazorPages();

// Register SQLite Database
builder.Services.AddDbContext<PlacementDbContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();

// Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// Initialize Placement Database
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider
        .GetRequiredService<PlacementDbContext>();

    DbInitializer.Initialize(dbContext);
}

app.Run();
