using Application.Models;

namespace Application.Services
{
    public interface IAssetService
    {
        Task<AssetDto?> GetAssetByIdAsync(int id);
        Task<List<AssetDto>> GetAllAssetsAsync();
        Task AddAssetAsync(AssetDto assetDto);
        Task UpdateAssetAsync(AssetDto assetDto);
        Task DeleteAssetAsync(int id);
    }
}
