using System.ComponentModel.DataAnnotations;

namespace SellStocks.Domain.Entities
{
    public class PriceDb
    {
        [Key]
        public int id {  get; set; }
        public double price { get; set; }
        public DateTime date { get; set; }

    }
}
