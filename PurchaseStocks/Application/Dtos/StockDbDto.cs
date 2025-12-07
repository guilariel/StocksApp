using SellStocks.Application.Dtos;

namespace StocksApp.Application.Dtos
{
    public class StockDbDto
    {
        public string symbol { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public PriceDbDto price { get; set; }
        public StockDbDto(string symbol, string name, string description, PriceDbDto priceDbDto) 
        {
            this.symbol = symbol;
            this.name = name;
            this.description = description;
            this.price = priceDbDto;
        }
    }
}
