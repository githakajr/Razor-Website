using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Extensions;
using RazorWebsite.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // Add your DbSet properties here
    public DbSet<User> Users { get; set; } // Example of a DbSet for users
    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder); // Call the base method

        // Configure your entities here
        modelBuilder.Entity<User>()
            .HasKey(u => u.Id); // Example of setting the primary key

        // Further configuration can be done here
    }
}
