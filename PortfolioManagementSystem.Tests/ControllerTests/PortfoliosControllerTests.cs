using Application.Models;
using Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Presentation.Controllers;

namespace PortfolioManagementSystem.Tests.ControllerTests;
public class PortfoliosControllerTests
{
    private readonly PortfolioController _controller;
    private readonly Mock<IPortfolioService> _portfolioServiceMock;

    public PortfoliosControllerTests()
    {
        _portfolioServiceMock = new Mock<IPortfolioService>();
        _controller = new PortfolioController(_portfolioServiceMock.Object);
    }

    [Fact]
    public void Controller_Should_Have_Correct_Route()
    {
        // Arrange: Get the type of the controller
        var controllerType = typeof(PortfolioController);

        // Act: Get the Route attribute applied on the controller
        var routeAttribute = controllerType.GetCustomAttributes(typeof(RouteAttribute), false).FirstOrDefault() as RouteAttribute;

        // Assert: Check if the Route attribute exists and matches the expected route
        Assert.NotNull(routeAttribute);
        Assert.Equal("api/[controller]", routeAttribute.Template);
    }

    [Fact]
    public async Task GetPortfolio_ReturnsOk_WithPortfolioData()
    {
        // Arrange
        var portfolioDto = new PortfolioDto
        {
            Id = 1,
            Name = "Test Portfolio"
        };

        _portfolioServiceMock.Setup(service => service.GetPortfolioByIdAsync(1))
            .ReturnsAsync(portfolioDto);

        // Act
        var result = await _controller.GetPortfolio(1) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<OkObjectResult>(result);
        var returnedPortfolio = result.Value as PortfolioDto;
        Assert.Equal(portfolioDto, returnedPortfolio);
    }

    [Fact]
    public async Task GetPortfolio_ReturnsNotFound_WhenPortfolioDoesNotExist()
    {
        // Arrange
        _portfolioServiceMock.Setup(service => service.GetPortfolioByIdAsync(1))
            .ReturnsAsync((PortfolioDto?)null);

        // Act
        var result = await _controller.GetPortfolio(1);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task GetPortfolioMatrics_ReturnsOk_WithMatrics()
    {
        // Arrange
        var portfolioId = 1;

        var portfolioDto = new PortfolioDto
        {
            Id = portfolioId,
            Name = "Sample Portfolio",
            Assets = new List<AssetDto>
        {
            new AssetDto
            {
                Id = 1,
                Name = "Asset 1",
                Type = "Stock",
                CurrentMarketValue = 10000,
                CostBasis = 8000,
                QuantityHeld = 10,
                PortfolioId = portfolioId
            },
            new AssetDto
            {
                Id = 2,
                Name = "Asset 2",
                Type = "Bond",
                CurrentMarketValue = 5000,
                CostBasis = 4000,
                QuantityHeld = 5,
                PortfolioId = portfolioId
            }
        }
        };

        var portfolioMetrics = new PortfolioMetricsDto
        {
            TotalMarketValue = 15000,
            PortfolioROI = 25,
            PortfolioRisk = 0
        };

        _portfolioServiceMock.Setup(s => s.GetPortfolioByIdAsync(portfolioId))
            .ReturnsAsync(portfolioDto);

        _portfolioServiceMock.Setup(s => s.GetPortfolioMetrics(portfolioDto))
            .Returns(portfolioMetrics);

        // Act
        var result = await _controller.GetPortfolioMetrics(portfolioId) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result!.StatusCode.Should().Be(200);

        var metrics = result.Value as PortfolioMetricsDto;
        metrics.Should().NotBeNull();
        metrics!.TotalMarketValue.Should().Be(portfolioMetrics.TotalMarketValue);
        metrics.PortfolioROI.Should().Be(portfolioMetrics.PortfolioROI);
        metrics.PortfolioRisk.Should().Be(portfolioMetrics.PortfolioRisk);
    }

    [Fact]
    public async Task GetPortfolioRisk_ReturnsOk_WithRisk()
    {
        // Arrange
        var portfolioDto = new PortfolioDto { Id = 1 };
        var portfolioMetricsDto = new PortfolioMetricsDto { PortfolioRisk = 1 };

        _portfolioServiceMock.Setup(service => service.GetPortfolioMetrics(portfolioDto))
            .Returns(portfolioMetricsDto);

        _portfolioServiceMock.Setup(service => service.GetPortfolioByIdAsync(1))
            .ReturnsAsync(portfolioDto);

        // Act
        var result = await _controller.GetPortfolioRisk(1) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
    }
}
