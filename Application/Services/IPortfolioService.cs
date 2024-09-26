using Application.Models;
using Domain.Helper;
namespace Application.Services
{
    public interface IPortfolioService
    {
        Task<PortfolioDto?> GetPortfolioByIdAsync(int id);
        Task<List<PortfolioDto>> GetAllPortfoliosAsync();
        Task AddPortfolioAsync(PortfolioDto portfolioDto);
        Task UpdatePortfolioAsync(PortfolioDto portfolioDto);
        Task DeletePortfolioAsync(int id);
        Task<PaginatedResult<TransactionsDto>> GetPortfolioTransactionsAsync(int portfolioId, string? transactionType, DateTime? startDate, DateTime? endDate, int pageNumber, int pageSize);
        Task<decimal> GetAnnualizedReturnAsync(int portfolioId);
        PortfolioMetricsDto GetPortfolioMetrics(PortfolioDto portfolioDto);
    }
}
