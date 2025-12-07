using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksApp.Domain.Entities.DbEntities
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
