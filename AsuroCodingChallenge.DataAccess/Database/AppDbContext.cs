using AsuroCodingChallenge.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace AsuroCodingChallenge.DataAccess.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<FileUploadRecord> FileUploadRecords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileUploadRecord>()
               .HasKey(fr => fr.TrackingId);
        }
    }
}
