using System.ComponentModel.DataAnnotations;

namespace SellStocks.Domain.Entities
{
    public class UsersDb
    {
        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public double funds { get; set; }
        public string password_hash { get; set; }

    }
}
