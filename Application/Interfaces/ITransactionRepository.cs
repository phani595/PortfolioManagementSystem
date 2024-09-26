using Domain.Entities;
using Domain.Helper;

namespace Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(int id);
        Task<List<Transaction>> GetAllAsync();
        Task AddAsync(Transaction transaction);
        Task UpdateAsync(Transaction transaction);
        Task DeleteAsync(int id);
        Task<PaginatedResult<Transaction>> GetTransactionsByPortfolioIdAsync(
         int portfolioId,
         string? transactionType,
         DateTime? startDate,
         DateTime? endDate,
         int pageNumber,
         int pageSize);
    }
}
