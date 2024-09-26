using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.ScheduledServices
{
    public class AssetPriceUpdateService : IAssetPriceUpdateService
    {
        private readonly ILogger<AssetPriceUpdateService> _logger;
        private readonly IAssetRepository _assetRepository;

        public AssetPriceUpdateService(ILogger<AssetPriceUpdateService> logger, IAssetRepository assetRepository)
        {
            _logger = logger;
            _assetRepository = assetRepository;
        }

        public async Task UpdateAssetPricesAsync()
        {
            _logger.LogInformation("Starting asset price update.");

            var assets = await _assetRepository.GetAllAsync();

            // API call to get the latest prices
            foreach (var asset in assets)
            {
                asset.CurrentMarketValue += GetPriceUpdate(); // Dummy price update logic
                await _assetRepository.UpdateAsync(asset);
            }

            _logger.LogInformation("Asset prices updated successfully.");
        }

        private decimal GetPriceUpdate()
        {
            //dummy value for price update
            return new Random().Next(1, 100);
        }
    }
}
