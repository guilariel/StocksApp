namespace StocksApp.Application.Dtos.RepositoryDtos
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
