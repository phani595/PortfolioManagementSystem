namespace Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Type { get; set; } // Buy, Sell
        public decimal Price { get; set; } // Price per unit
        public int Quantity { get; set; }
        public decimal Fees { get; set; }

        public int AssetId { get; set; }

        // Navigation property
        public virtual Asset Asset { get; set; }

    }

}
