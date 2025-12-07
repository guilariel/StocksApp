using System.ComponentModel.DataAnnotations;

namespace StocksApp.Application.Dtos.DbDtos
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
