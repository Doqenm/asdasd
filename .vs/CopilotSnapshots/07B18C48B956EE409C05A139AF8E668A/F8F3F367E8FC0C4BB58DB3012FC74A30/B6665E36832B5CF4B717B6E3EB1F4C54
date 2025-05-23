using Microsoft.EntityFrameworkCore;
using Managing.Models;

namespace Managing.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<MaterialReceipt> MaterialReceipts { get; set; }
    public DbSet<ProductionOrder> ProductionOrders { get; set; }
    public DbSet<InventoryItem> InventoryItems { get; set; }
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Configurações adicionais, índices, relações, seeds...
    }
}
