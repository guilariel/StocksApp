using System.ComponentModel.DataAnnotations;

namespace SellStocks.Application.Dtos
{
    public class UsersDbAddDto
    {
        public string Name { get; set; }
        public double Funds { get; set; }
        public string password_hash { get; set; }
        public UsersDbAddDto() { }

        public UsersDbAddDto(string Name, double Funds, string password_hash)
        {
            this.Name = Name;
            this.Funds = Funds;
            this.password_hash = password_hash;
        }
    }
}
