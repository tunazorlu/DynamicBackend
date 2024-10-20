using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Shared.Entities;

namespace Backend.Data;

public class DynamicDbContext(DbContextOptions<DynamicDbContext> options) : DbContext(options)
{
    public DbSet<BaseEntity> Entities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tüm entity'leri dinamik olarak yapılandır
        base.OnModelCreating(modelBuilder);
    }

    public void AddEntityType(string entityName, Dictionary<string, Type> properties, ModelBuilder modelBuilder)
    {
        var entityBuilder = modelBuilder.Entity(entityName);
        entityBuilder.HasKey("Id");

        foreach (var property in properties)
        {
            entityBuilder.Property(property.Value, property.Key);
        }
    }

    // Migration'u kontrol edip uygular
    public async Task ApplyMigration()
    {
        var pendingMigrations = await Database.GetPendingMigrationsAsync();
        if (pendingMigrations.Any())
        {
            await Database.MigrateAsync();
        }
    }
}