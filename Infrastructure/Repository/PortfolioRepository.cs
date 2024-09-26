using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly PortfolioManagementDbContext _context;

        public PortfolioRepository(PortfolioManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Portfolio?> GetByIdAsync(int id)
        {
            return await _context.Set<Portfolio>()
                .Include(p => p.Assets)
                .ThenInclude(a => a.Transactions)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Portfolio>> GetAllAsync()
        {
            return await _context.Set<Portfolio>()
                .Include(p => p.Assets)
                .ToListAsync();
        }

        public async Task AddAsync(Portfolio portfolio)
        {
            await _context.Set<Portfolio>().AddAsync(portfolio);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Portfolio portfolio)
        {
            _context.Set<Portfolio>().Update(portfolio);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var portfolio = await _context.Set<Portfolio>().FindAsync(id);
            if (portfolio != null)
            {
                _context.Set<Portfolio>().Remove(portfolio);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<decimal> GetAnnualizedReturnAsync(int portfolioId)
        {
            var portfolio = await _context.Portfolios
                .Include(p => p.Assets)
                .ThenInclude(a => a.Transactions)
                .FirstOrDefaultAsync(p => p.Id == portfolioId);

            if (portfolio == null)
                throw new ArgumentException("Portfolio not found.");

            decimal initialInvestment = 0;
            decimal currentMarketValue = 0;

            foreach (var asset in portfolio.Assets)
            {
                initialInvestment += asset.CostBasis * asset.QuantityHeld;
                currentMarketValue += asset.CurrentMarketValue * asset.QuantityHeld;
            }


            var earliestTransaction = portfolio.Assets
                .SelectMany(a => a.Transactions)
                .OrderBy(t => t.TransactionDate)
                .FirstOrDefault();

            if (earliestTransaction == null)
                throw new InvalidOperationException("No transactions found for this portfolio.");

            var yearsHeld = (DateTime.Now - earliestTransaction.TransactionDate).TotalDays / 365;

            if (yearsHeld == 0)
                throw new InvalidOperationException("Portfolio held for less than one year.");


            var annualizedReturn = (decimal)Math.Pow((double)(currentMarketValue / initialInvestment), (1 / yearsHeld)) - 1;

            return annualizedReturn;
        }

    }
}
