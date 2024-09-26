using Application.Interfaces;
using Application.Models;
using Domain.Entities;

namespace Application.Services
{
    public class TransactionService : ITransactionService
    {

        private readonly ITransactionRepository _transactionRepository;
        private readonly IAssetRepository _assetRepository;

        public TransactionService(ITransactionRepository transactionRepository, IAssetRepository assetRepository)
        {
            _transactionRepository = transactionRepository;
            _assetRepository = assetRepository;
        }

        public async Task<TransactionsDto?> GetTransactionByIdAsync(int id)
        {
            var transaction = await _transactionRepository.GetByIdAsync(id);
            if (transaction == null) return null;

            return new TransactionsDto
            {
                Id = transaction.Id,
                TransactionDate = transaction.TransactionDate,
                Type = transaction.Type,
                Price = transaction.Price,
                Quantity = transaction.Quantity,
                Fees = transaction.Fees,
                AssetId = transaction.AssetId
            };
        }

        public async Task<List<TransactionsDto>> GetAllTransactionsAsync()
        {
            var transactions = await _transactionRepository.GetAllAsync();
            return transactions.Select(transaction => new TransactionsDto
            {
                Id = transaction.Id,
                TransactionDate = transaction.TransactionDate,
                Type = transaction.Type,
                Price = transaction.Price,
                Quantity = transaction.Quantity,
                Fees = transaction.Fees,
                AssetId = transaction.AssetId
            }).ToList();
        }

        public async Task AddTransactionAsync(TransactionsDto transactionDto)
        {
            var asset = await _assetRepository.GetByIdAsync(transactionDto.AssetId);
            if (asset == null)
            {
                throw new Exception("Asset not found.");
            }

            var transaction = new Transaction
            {
                TransactionDate = transactionDto.TransactionDate,
                Type = transactionDto.Type,
                Price = transactionDto.Price,
                Quantity = transactionDto.Quantity,
                Fees = transactionDto.Fees,
                AssetId = transactionDto.AssetId
            };

            await _transactionRepository.AddAsync(transaction);
        }

        public async Task UpdateTransactionAsync(TransactionsDto transactionDto)
        {
            var asset = await _assetRepository.GetByIdAsync(transactionDto.AssetId);
            if (asset == null)
            {
                throw new Exception("Asset not found.");
            }

            var transaction = new Transaction
            {
                Id = transactionDto.Id,
                TransactionDate = transactionDto.TransactionDate,
                Type = transactionDto.Type,
                Price = transactionDto.Price,
                Quantity = transactionDto.Quantity,
                Fees = transactionDto.Fees,
                AssetId = transactionDto.AssetId
            };

            await _transactionRepository.UpdateAsync(transaction);
        }

        public async Task DeleteTransactionAsync(int id)
        {
            await _transactionRepository.DeleteAsync(id);
        }

    }
}
