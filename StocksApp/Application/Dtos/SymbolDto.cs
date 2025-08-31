namespace StocksApp.Application.Dtos
{
    public class SymbolDto
    {
        public string Symbol { get; set; }
        public static SymbolDto FromEntity(string symbol)
        {
            return new SymbolDto { Symbol = symbol };
        }
    }
}
