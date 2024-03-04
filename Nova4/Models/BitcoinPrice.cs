namespace Nova4.Models
{
    public class BitcoinPrice
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; } 
        public string Source { get; set; } 
    }
}
