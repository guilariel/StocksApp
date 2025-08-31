namespace StocksApp.Application.Dtos
{
    public class StockDbDto
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Company_Id { get; set; }

    }
}
