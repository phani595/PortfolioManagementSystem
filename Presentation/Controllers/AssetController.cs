using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;

        public AssetsController(IAssetService assetService)
        {
            _assetService = assetService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAssetById(int id)
        {
            var asset = await _assetService.GetAssetByIdAsync(id);
            if (asset == null) return NotFound();
            return Ok(asset);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssets()
        {
            var assets = await _assetService.GetAllAssetsAsync();
            return Ok(assets);
        }
        [Authorize(Roles = "Advisor, Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAsset([FromBody] AssetDto assetDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _assetService.AddAssetAsync(assetDto);
            return CreatedAtAction(nameof(GetAssetById), new { id = assetDto.Id }, assetDto);
        }
        [Authorize(Roles = "Advisor, Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsset(int id, [FromBody] AssetDto assetDto)
        {
            if (id != assetDto.Id) return BadRequest();
            await _assetService.UpdateAssetAsync(assetDto);
            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            await _assetService.DeleteAssetAsync(id);
            return NoContent();
        }

    }
}
