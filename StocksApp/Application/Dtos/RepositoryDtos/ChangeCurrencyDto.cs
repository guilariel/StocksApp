namespace StocksApp.Application.Dtos.RepositoryDtos
{
    public class ChangeCurrencyDto
    {
        public string Symbol { get; set; }
        public string Currency { get; set; }
        public static ChangeCurrencyDto FromEntity(string symbol, string currency)
        {
            return new ChangeCurrencyDto
            {
				Symbol = symbol,
                Currency = currency
            };
        }
    }
}
