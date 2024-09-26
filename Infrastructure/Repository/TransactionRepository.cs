using Application.Interfaces;
using Domain.Entities;
using Domain.Helper;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class TransactionRepository : ITransactionRepository
    {

        private readonly PortfolioManagementDbContext _context;

        public TransactionRepository(PortfolioManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetByIdAsync(int id)
        {
            return await _context.Set<Transaction>().FindAsync(id);
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Set<Transaction>().ToListAsync();
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Set<Transaction>().AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Transaction transaction)
        {
            _context.Set<Transaction>().Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var transaction = await _context.Set<Transaction>().FindAsync(id);
            if (transaction != null)
            {
                _context.Set<Transaction>().Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<PaginatedResult<Transaction>> GetTransactionsByPortfolioIdAsync(
        int portfolioId,
        string? transactionType,
        DateTime? startDate,
        DateTime? endDate,
        int pageNumber,
        int pageSize)
        {
            var query = _context.Set<Transaction>()
                .Where(t => t.Asset.PortfolioId == portfolioId)
                .AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(transactionType))
            {
                query = query.Where(t => t.Type == transactionType);
            }

            if (startDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate <= endDate.Value);
            }

            // Pagination
            var totalCount = await query.CountAsync();
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginatedResult<Transaction>(items, totalCount, pageNumber, pageSize);
        }
    }
}
