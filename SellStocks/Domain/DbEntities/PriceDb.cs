using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellStocks.Domain.Entities
{
    public class PriceDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id {  get; set; }
        public double price { get; set; }
        public DateTime date { get; set; }

    }
}
