using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Shared.Entities;

namespace Backend.Data;
public class DynamicDbContext : DbContext
{
    public DynamicDbContext(DbContextOptions<DynamicDbContext> options) : base(options)
    {
    }

    public DbSet<MetaTable> MetaTables { get; set; }
    public DbSet<MetaColumn> MetaColumns { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MetaTable>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<MetaColumn>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.HasOne(e => e.MetaTable)
                .WithMany(e => e.Columns)
                .HasForeignKey(e => e.MetaTableId);
        });
    }
}