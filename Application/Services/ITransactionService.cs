using Application.Models;

namespace Application.Services
{
    public interface ITransactionService
    {

        Task<TransactionsDto> GetTransactionByIdAsync(int id);
        Task<List<TransactionsDto>> GetAllTransactionsAsync();
        Task AddTransactionAsync(TransactionsDto transactionDto);
        Task UpdateTransactionAsync(TransactionsDto transactionDto);
        Task DeleteTransactionAsync(int id);
    }
}
