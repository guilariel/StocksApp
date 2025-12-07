using System.ComponentModel.DataAnnotations;

namespace SellStocks.Domain.Entities
{
    public class StockDb
    {
        [Key]
        public int id {  get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int price_id { get; set; }

    }
}
