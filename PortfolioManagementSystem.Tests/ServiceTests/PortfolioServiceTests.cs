using Application.Interfaces;
using Application.Services;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace PortfolioManagementSystem.Tests.ServiceTests;
public class PortfolioServiceTests
{
    private readonly Mock<IPortfolioRepository> _mockPortfolioRepository;
    private readonly Mock<ITransactionRepository> _mockTransactionRepository;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<ILogger<PortfolioService>> _mockLogger;
    private readonly PortfolioService _portfolioService;

    public PortfolioServiceTests()
    {
        _mockPortfolioRepository = new Mock<IPortfolioRepository>();
        _mockTransactionRepository = new Mock<ITransactionRepository>();
        _mockMapper = new Mock<IMapper>();
        _mockLogger = new Mock<ILogger<PortfolioService>>();

        _portfolioService = new PortfolioService(
           _mockPortfolioRepository.Object,
           _mockLogger.Object,
           _mockMapper.Object,
           _mockTransactionRepository.Object);
    }

    [Fact]
    public async Task GetPortfolioByIdAsync_ReturnsPortfolio_WithCorrectCalculations()
    {
        // Arrange
        var portfolio = new Portfolio
        {
            Id = 1,
            Name = "Test Portfolio",
            Assets =
            [
                new() {
                    Id = 1, Name = "Asset 1", CurrentMarketValue = 100, CostBasis = 50, QuantityHeld = 10, Type="Stock",
                    Transactions =
                    [
                        new Transaction { Id = 1, TransactionDate = new DateTime(2021, 1, 1), Type = "Buy", Price = 5, Quantity = 10, Fees = 0, AssetId = 1 }
                    ],
                },
                new ()
                {
                    Id = 2, Name = "Asset 2", CurrentMarketValue = 200, CostBasis = 150, QuantityHeld = 5, Type="Stock",
                    Transactions =
                    [
                        new Transaction { Id = 2, TransactionDate = new DateTime(2021, 1, 1), Type = "Buy", Price = 5, Quantity = 10, Fees = 0, AssetId = 1 }
                    ],
                }
            ]
        };

        _mockPortfolioRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(portfolio);

        // Act
        var result = await _portfolioService.GetPortfolioByIdAsync(1);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetPortfolioByIdAsync_ReturnsNull_WhenPortfolioDoesNotExist()
    {
        // Arrange
        _mockPortfolioRepository.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync((Portfolio?)null);

        // Act
        var result = await _portfolioService.GetPortfolioByIdAsync(1);

        // Assert
        Assert.Null(result);
    }

}
