using Application.Models;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;

namespace PortfolioManagementSystem.Tests.ControllerTests;
public class AssetControllerTests
{
    private readonly AssetsController _controller;
    private readonly Mock<IAssetService> _assetServiceMock;

    public AssetControllerTests()
    {
        _assetServiceMock = new Mock<IAssetService>();
        _controller = new AssetsController(_assetServiceMock.Object);
    }


    [Fact]
    public async Task GetAssetById_ReturnsOk_WithAsset()
    {
        // Arrange
        var assetId = 1;
        var asset = new AssetDto { Id = 1, Name = "Asset 1", CurrentMarketValue = 100, CostBasis = 80 };

        _assetServiceMock.Setup(service => service.GetAssetByIdAsync(assetId)).ReturnsAsync(asset);

        // Act
        var result = await _controller.GetAssetById(assetId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        var returnedAsset = result.Value as AssetDto;
        Assert.Equal(100, returnedAsset.CurrentMarketValue);
        Assert.Equal(80, returnedAsset.CostBasis);
    }

    [Fact]
    public async Task GetAssetById_ReturnsNotFound_WhenAssetDoesNotExist()
    {
        // Arrange
        var assetId = 1;
        _assetServiceMock.Setup(service => service.GetAssetByIdAsync(assetId)).ReturnsAsync((AssetDto)null);

        // Act
        var result = await _controller.GetAssetById(assetId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
