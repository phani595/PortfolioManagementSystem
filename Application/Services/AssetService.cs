using Application.Interfaces;
using Application.Models;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ILogger<AssetService> _logger;

        public AssetService(IAssetRepository assetRepository, IPortfolioRepository portfolioRepository, ILogger<AssetService> logger)
        {
            _assetRepository = assetRepository;
            _portfolioRepository = portfolioRepository;
            _logger = logger;
        }

        public async Task<AssetDto?> GetAssetByIdAsync(int id)
        {
            var asset = await _assetRepository.GetByIdAsync(id);
            return asset == null ? null : new AssetDto
            {
                Id = asset.Id,
                Name = asset.Name,
                Type = asset.Type,
                CurrentMarketValue = asset.CurrentMarketValue,
                CostBasis = asset.CostBasis,
                QuantityHeld = asset.QuantityHeld,
                PortfolioId = asset.PortfolioId
            };
        }

        public async Task<List<AssetDto>> GetAllAssetsAsync()
        {
            var assets = await _assetRepository.GetAllAsync();
            return assets.Select(asset => new AssetDto
            {
                Id = asset.Id,
                Name = asset.Name,
                Type = asset.Type,
                CurrentMarketValue = asset.CurrentMarketValue,
                CostBasis = asset.CostBasis,
                QuantityHeld = asset.QuantityHeld,
                PortfolioId = asset.PortfolioId
            }).ToList();
        }

        public async Task AddAssetAsync(AssetDto assetDto)
        {
            var asset = new Asset
            {
                Name = assetDto.Name,
                Type = assetDto.Type,
                CurrentMarketValue = assetDto.CurrentMarketValue,
                CostBasis = assetDto.CostBasis,
                QuantityHeld = assetDto.QuantityHeld,
                PortfolioId = assetDto.PortfolioId
            };
            await _assetRepository.AddAsync(asset);
        }

        public async Task UpdateAssetAsync(AssetDto assetDto)
        {
            var asset = new Asset
            {
                Id = assetDto.Id,
                Name = assetDto.Name,
                Type = assetDto.Type,
                CurrentMarketValue = assetDto.CurrentMarketValue,
                CostBasis = assetDto.CostBasis,
                QuantityHeld = assetDto.QuantityHeld,
                PortfolioId = assetDto.PortfolioId
            };
            await _assetRepository.UpdateAsync(asset);
        }

        public async Task DeleteAssetAsync(int id)
        {
            await _assetRepository.DeleteAsync(id);
        }

    }
}
