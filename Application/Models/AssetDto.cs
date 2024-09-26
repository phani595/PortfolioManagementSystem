namespace Application.Models
{
    public class AssetDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Type { get; set; } = default!;
        public decimal CurrentMarketValue { get; set; }
        public decimal CostBasis { get; set; }
        public int QuantityHeld { get; set; }
        public int PortfolioId { get; set; } // Link to Portfolio

        public List<TransactionsDto> Transactions { get; set; } = new();

    }
}
