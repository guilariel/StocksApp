using System.Globalization;

namespace StocksApp.Infrastructure.Utilities
{
    public class CurrencyFormater
    {
        private static readonly Dictionary<string, string> CurrencyCultureMap = new()
        {
            { "USD", "en-US" },
            { "EUR", "fr-FR" }
        };

        public static string FormatCurrency(decimal amount, string currency)
        {
            if (!CurrencyCultureMap.TryGetValue(currency, out var cultureName))
            {
                throw new ArgumentException("The currency doesn't exist.");
            }

            var culture = new CultureInfo(cultureName);
            return amount.ToString("C", culture);
        }
    }
}