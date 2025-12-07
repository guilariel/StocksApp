using System.ComponentModel.DataAnnotations;

namespace StocksApp.Application.Dtos.DbDtos
{
    public class InPossessionDbDto
    {
        public UsersDbDto Owner { get; set; }
        public StockDbDto Stock { get; set; }
        public int amount { get; set; }
        public InPossessionDbDto(UsersDbDto usersDbDto, StockDbDto stockDbDto, int amount) 
        {
            Owner = usersDbDto;
            Stock = stockDbDto;
            this.amount = amount;
        }
    }
}
