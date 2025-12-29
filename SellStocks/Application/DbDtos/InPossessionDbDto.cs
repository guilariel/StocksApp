using System.ComponentModel.DataAnnotations;
using SellStocks.Application.Dtos;
using StocksApp.Application.Dtos; // Asegúrate de incluir el espacio de nombres correcto para UsersDbDto

namespace SellStocks.Application.Dtos
{
    public class InPossessionDbDto
    {
        public int owner_id { get; set; }
        public int stock_id { get; set; }
        public int amount { get; set; }
    }
}
