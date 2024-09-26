using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class PortfolioManagementDbContext(DbContextOptions<PortfolioManagementDbContext> options) : DbContext(options)
    {
        internal DbSet<User> Users { get; set; }
        internal DbSet<Portfolio> Portfolios { get; set; }
        internal DbSet<Asset> Assets { get; set; }
        internal DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Portfolio>()
            .HasMany(p => p.Assets)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade); // If a portfolio is deleted, its assets are deleted

            modelBuilder.Entity<Asset>()
                .HasMany(a => a.Transactions)
                .WithOne(t => t.Asset)
                .HasForeignKey(t => t.AssetId)
                .OnDelete(DeleteBehavior.Cascade); // If an asset is deleted, its transactions are deleted

            base.OnModelCreating(modelBuilder);

        }
    }

}
