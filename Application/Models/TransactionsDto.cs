﻿namespace Application.Models
{
    public class TransactionsDto
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Fees { get; set; }
        public int AssetId { get; set; }
    }
}
