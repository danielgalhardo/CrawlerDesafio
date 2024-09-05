using Microsoft.EntityFrameworkCore;
using CrawlerAlura.src.Domain.Entities;

namespace CrawlerAlura.src.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<AluraCourse> AluraCourses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AluraCourse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(255);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.Link).HasMaxLength(255);
                entity.Property(e => e.Instructor).HasMaxLength(500);
                entity.Property(e => e.Workload).HasMaxLength(10);
            });
        }
    }
}
