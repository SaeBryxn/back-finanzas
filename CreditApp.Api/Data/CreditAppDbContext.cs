using CreditApp.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CreditApp.Api.Data;

public class CreditAppDbContext(DbContextOptions<CreditAppDbContext> options) : DbContext(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Unit> Units => Set<Unit>();
    public DbSet<Config> Configs => Set<Config>();
    public DbSet<Simulation> Simulations => Set<Simulation>();
    public DbSet<AuditLog> Audit => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Decimal a numeric(18,2) por defecto
        foreach (var prop in modelBuilder.Model
                     .GetEntityTypes()
                     .SelectMany(t => t.GetProperties())
                     .Where(p => p.ClrType == typeof(decimal)))
        {
            prop.SetPrecision(18);
            prop.SetScale(2);
        }

        base.OnModelCreating(modelBuilder);
    }
}