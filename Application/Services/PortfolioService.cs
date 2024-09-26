using Application.Interfaces;
using Application.Models;
using AutoMapper;
using Domain.Entities;
using Domain.Helper;
using Microsoft.Extensions.Logging;


namespace Application.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly IPortfolioRepository _portfolioRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PortfolioService> _logger;

        public PortfolioService(IPortfolioRepository portfolioRepository, ILogger<PortfolioService> logger, IMapper mapper, ITransactionRepository transactionRepository)
        {
            _portfolioRepository = portfolioRepository;
            _logger = logger;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
        public async Task<PortfolioDto?> GetPortfolioByIdAsync(int id)
        {
            _logger.LogInformation("Getting total value for portfolio ID: {PortfolioId}", id);

            var portfolio = await _portfolioRepository.GetByIdAsync(id);
            if (portfolio == null) return null;

            var portfolioDto = new PortfolioDto
            {
                Id = portfolio.Id,
                Name = portfolio.Name,
                Assets = portfolio.Assets.Select(asset => new AssetDto
                {
                    Id = asset.Id,
                    Name = asset.Name,
                    Type = asset.Type,
                    CurrentMarketValue = asset.CurrentMarketValue,
                    CostBasis = asset.CostBasis,
                    QuantityHeld = asset.QuantityHeld,
                    PortfolioId = asset.PortfolioId,
                    Transactions = asset.Transactions.Select(transaction => new TransactionsDto
                    {
                        Id = transaction.Id,
                        TransactionDate = transaction.TransactionDate,
                        Type = transaction.Type,
                        Price = transaction.Price,
                        Quantity = transaction.Quantity,
                        Fees = transaction.Fees,
                        AssetId = transaction.AssetId
                    }).ToList()
                }).ToList()
            };

            return portfolioDto;
        }

        public async Task<List<PortfolioDto>> GetAllPortfoliosAsync()
        {
            var portfolios = await _portfolioRepository.GetAllAsync();
            return portfolios.Select(portfolio => new PortfolioDto
            {
                Id = portfolio.Id,
                Name = portfolio.Name,
                Assets = portfolio.Assets.Select(asset => new AssetDto
                {
                    Id = asset.Id,
                    Name = asset.Name,
                    Type = asset.Type,
                    CurrentMarketValue = asset.CurrentMarketValue,
                    CostBasis = asset.CostBasis,
                    QuantityHeld = asset.QuantityHeld,
                    PortfolioId = asset.PortfolioId
                }).ToList()
            }).ToList();
        }

        public async Task AddPortfolioAsync(PortfolioDto portfolioDto)
        {
            var portfolio = new Portfolio
            {
                Name = portfolioDto.Name,
                Assets = new List<Asset>()
            };

            // If Assets are provided
            if (portfolioDto.Assets != null && portfolioDto.Assets.Any())
            {
                foreach (var assetDto in portfolioDto.Assets)
                {
                    var asset = new Asset
                    {
                        Name = assetDto.Name,
                        Type = assetDto.Type,
                        CurrentMarketValue = assetDto.CurrentMarketValue,
                        CostBasis = assetDto.CostBasis,
                        QuantityHeld = assetDto.QuantityHeld,
                        Transactions = new List<Transaction>()
                    };

                    // If Transactions are provided
                    if (assetDto.Transactions != null && assetDto.Transactions.Any())
                    {
                        foreach (var transactionDto in assetDto.Transactions)
                        {
                            var transaction = new Transaction
                            {
                                TransactionDate = transactionDto.TransactionDate,
                                Type = transactionDto.Type,
                                Price = transactionDto.Price,
                                Quantity = transactionDto.Quantity,
                                Fees = transactionDto.Fees,
                            };

                            asset.Transactions.Add(transaction); // Add transaction to asset
                        }
                    }

                    portfolio.Assets.Add(asset); // Add asset to portfolio
                }
            }



            await _portfolioRepository.AddAsync(portfolio);
        }

        public async Task UpdatePortfolioAsync(PortfolioDto portfolioDto)
        {
            var portfolio = new Portfolio
            {
                Id = portfolioDto.Id,
                Name = portfolioDto.Name
            };

            await _portfolioRepository.UpdateAsync(portfolio);
        }

        public async Task DeletePortfolioAsync(int id)
        {
            await _portfolioRepository.DeleteAsync(id);
        }

        public PortfolioMetricsDto GetPortfolioMetrics(PortfolioDto portfolioDto)
        {
            var totalMarketValue = CalculateTotalMarketValue(portfolioDto.Assets);
            var portfolioROI = CalculatePortfolioROI(portfolioDto.Assets);
            var portfolioRisk = CalculatePortfolioRisk(portfolioDto.Assets);

            return new PortfolioMetricsDto
            {
                TotalMarketValue = totalMarketValue,
                PortfolioROI = portfolioROI,
                PortfolioRisk = portfolioRisk
            };
        }

        private decimal CalculateTotalMarketValue(List<AssetDto> assets)
        {
            return assets.Sum(a => a.QuantityHeld * a.CurrentMarketValue);
        }

        private decimal CalculatePortfolioROI(List<AssetDto> assets)
        {
            var totalCostBasis = assets.Sum(asset => asset.QuantityHeld * asset.CostBasis);

            var totalMarketValue = assets.Sum(asset => asset.QuantityHeld * asset.CurrentMarketValue);

            if (totalCostBasis == 0) return 0;

            return ((totalMarketValue - totalCostBasis) / totalCostBasis) * 100;
        }


        private decimal CalculatePortfolioRisk(List<AssetDto> assets)
        {
            return 0;
        }

        public async Task<PaginatedResult<TransactionsDto>> GetPortfolioTransactionsAsync(int portfolioId, string? transactionType, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize)
        {
            var portfolio = await _portfolioRepository.GetByIdAsync(portfolioId);
            if (portfolio == null)
            {
                _logger.LogWarning("Portfolio with id {portfolioId} not found", portfolioId);
                return null;
            }

            var transactions = await _transactionRepository.GetTransactionsByPortfolioIdAsync(
                portfolioId, transactionType, startDate, endDate, pageNumber, pageSize);

            var mappedTransactions = _mapper.Map<List<TransactionsDto>>(transactions.Items);

            return new PaginatedResult<TransactionsDto>(mappedTransactions, transactions.TotalCount, pageNumber, pageSize);
        }

        public async Task<decimal> GetAnnualizedReturnAsync(int portfolioId)
        {
            try
            {
                return await _portfolioRepository.GetAnnualizedReturnAsync(portfolioId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating annualized return for portfolio {PortfolioId}", portfolioId);
                throw;
            }
        }

    }
}
