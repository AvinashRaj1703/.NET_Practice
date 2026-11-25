
using Microsoft.EntityFrameworkCore;
using PlacementDashboard.Models;

namespace PlacementDashboard.Data
{
    public class PlacementDbContext : DbContext
    {
        public PlacementDbContext(DbContextOptions<PlacementDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Placement> Placements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Placement>()
                .HasOne(p => p.Student)
                .WithMany()
                .HasForeignKey(p => p.StudentID);

            modelBuilder.Entity<Placement>()
                .HasOne(p => p.Company)
                .WithMany()
                .HasForeignKey(p => p.CompanyID);
        }
    }
}
