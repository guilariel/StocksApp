using System.ComponentModel.DataAnnotations;

namespace SellStocks.Application.Dtos
{
    public class PriceDbDto
    {
        public double price { get; set; }
        public DateTime date { get; set; }
        public PriceDbDto(double price, DateTime dateTime) 
        {
            this.price = price;
            date = dateTime;
        }
    }
}
