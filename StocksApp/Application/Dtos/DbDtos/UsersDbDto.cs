using System.ComponentModel.DataAnnotations;

namespace StocksApp.Application.Dtos.DbDtos
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
