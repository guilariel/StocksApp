using System.ComponentModel.DataAnnotations;

namespace SellStocks.Application.Dtos
{
    public class UsersDbDto
    {
        public string Name { get; set; }
        public UsersDbDto(string Name) 
        {
            this.Name = Name;
        }
    }
}
