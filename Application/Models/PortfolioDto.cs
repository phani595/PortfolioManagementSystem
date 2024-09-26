namespace Application.Models
{
    public class PortfolioDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public List<AssetDto> Assets { get; set; } = new();
    }
}
