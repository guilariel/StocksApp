using System.ComponentModel.DataAnnotations;
using SellStocks.Application.Dtos;
using StocksApp.Application.Dtos; // Asegúrate de incluir el espacio de nombres correcto para UsersDbDto

namespace SellStocks.Application.Dtos
{
    public class InPossessionDbDto
    {
        public UsersDbDto Owner { get; set; }
        public StockDbDto Stock { get; set; }
        public int amount { get; set; }
        public InPossessionDbDto(UsersDbDto usersDbDto, StockDbDto stockDbDto, int amount) 
        {
            this.Owner = usersDbDto;
            this.Stock = stockDbDto;
            this.amount = amount;
        }
    }
}
