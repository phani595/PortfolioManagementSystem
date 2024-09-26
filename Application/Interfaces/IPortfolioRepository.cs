using Domain.Entities;

namespace Application.Interfaces
{
    public interface IPortfolioRepository
    {
        Task<Portfolio?> GetByIdAsync(int id);
        Task<List<Portfolio>> GetAllAsync();
        Task AddAsync(Portfolio portfolio);
        Task UpdateAsync(Portfolio portfolio);
        Task DeleteAsync(int id);
        Task<decimal> GetAnnualizedReturnAsync(int portfolioId);
    }
}
