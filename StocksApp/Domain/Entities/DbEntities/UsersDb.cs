using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StocksApp.Domain.Entities.DbEntities
{
    public class UsersDb
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string name { get; set; }
        public double funds { get; set; }
        public string password_hash { get; set; }

    }
}
