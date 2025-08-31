namespace StocksApp.Domain.ValueObjects
{
    public class CurrencyExchange
    {
        public static readonly Dictionary<string, decimal> CurrencyPrice = new Dictionary<string, decimal>
        {
            { "USD", 1m },
            { "EUR", 1.15m }
        };

    }
    public static class CurrencyExchangeExtensions
    {
        public static decimal GetExchangeRate(this string currency)
        {
            return CurrencyExchange.CurrencyPrice[currency];
        }
        public static bool ContainsCoin(this string coin)
        {
            return CurrencyExchange.CurrencyPrice.ContainsKey(coin);
        }
    }
}