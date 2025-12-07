using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellStocks.Domain.Entities
{
    public class InPossessionDb
    {
        [Key]
        public int owner_id { get; set; }
        public int stock_id { get; set; }
        public int amount { get; set; }

    }
}
