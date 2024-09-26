using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class AssetRepository : IAssetRepository
    {
        private readonly PortfolioManagementDbContext _context;

        public AssetRepository(PortfolioManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Asset?> GetByIdAsync(int id)
        {
            return await _context.Set<Asset>().FindAsync(id);
        }

        public async Task<List<Asset>> GetAllAsync()
        {
            return await _context.Set<Asset>().ToListAsync();
        }

        public async Task AddAsync(Asset asset)
        {
            await _context.Set<Asset>().AddAsync(asset);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Asset asset)
        {
            _context.Set<Asset>().Update(asset);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var asset = await _context.Set<Asset>().FindAsync(id);
            if (asset != null)
            {
                _context.Set<Asset>().Remove(asset);
                await _context.SaveChangesAsync();
            }
        }

    }
}
