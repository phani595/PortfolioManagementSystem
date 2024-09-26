namespace Domain.Entities
{
    public class Portfolio
    {
        public int Id { get; set; }
        public string Name { get; set; }
        // Navigation properties
        public List<Asset> Assets { get; set; } = new();

    }
}
