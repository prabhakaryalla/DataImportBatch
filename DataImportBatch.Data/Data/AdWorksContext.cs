using DataImportBatch.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DataImportBatch.Data.Data;

public partial class AdWorksContext : DbContext
{
    public AdWorksContext()
    {
    }

    public AdWorksContext(DbContextOptions<AdWorksContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<TransactionHistory> TransactionHistories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.ListPrice).HasColumnType("money");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<TransactionHistory>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PK_TransactionHistory_TransactionID");

            entity.ToTable("TransactionHistory");

            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.ActualCost).HasColumnType("money");
            entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.TransactionDate).HasColumnType("datetime");
            entity.Property(e => e.TransactionType)
                .HasMaxLength(1)
                .IsFixedLength();

            entity.HasOne(d => d.Product).WithMany(p => p.TransactionHistories)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TransactionHistory_Product");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
