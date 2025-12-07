using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogIn.Domain.Entities
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
