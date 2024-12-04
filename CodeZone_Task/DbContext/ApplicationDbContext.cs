using CodeZone_Task.Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> optionsBuilder) : base(optionsBuilder)
    {
    }

    public virtual DbSet<Store> Stores { get; set; }
    public virtual DbSet<Item> Items { get; set; }
    public virtual DbSet<Purchase> Purchases { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}