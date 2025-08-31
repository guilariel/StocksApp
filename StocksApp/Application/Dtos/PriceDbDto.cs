namespace StocksApp.Application.Dtos
{
    public class PriceDbDto
    {
        public int Id { get; set; }
        public int Stock_Id { get; set; }
        public decimal Price { get; set; }
        public DateTime date { get; set; }
    }
}
