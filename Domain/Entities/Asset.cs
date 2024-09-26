namespace Domain.Entities
{
    public class Asset
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        public required string Type { get; set; } // e.g., Stock, Bond

        public decimal CurrentMarketValue { get; set; }

        public decimal CostBasis { get; set; } // Total cost basis of the asset

        public int QuantityHeld { get; set; } //Purchase price per unit

        public int PortfolioId { get; set; }

        // Navigation property
        public virtual Portfolio Portfolio { get; set; }

        // Navigation properties
        public virtual ICollection<Transaction>? Transactions { get; set; }

    }
}
