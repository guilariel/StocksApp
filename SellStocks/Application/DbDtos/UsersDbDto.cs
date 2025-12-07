using System.ComponentModel.DataAnnotations;

namespace SellStocks.Application.Dtos
{
    public class UsersDbDto
    {
        public string Name { get; set; }
        public double Funds { get; set; }
        public UsersDbDto(string Name, double Funds) 
        {
            this.Name = Name;
            this.Funds = Funds; 
        }
    }
}
