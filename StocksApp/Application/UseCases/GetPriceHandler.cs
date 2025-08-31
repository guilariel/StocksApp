using StocksApp.Domain.Repositorys;
using StocksApp.Domain.ValueObjects;
using StocksApp.Infrastructure.Utilities;

namespace StocksApp.Application.UseCases
{
    public class GetPriceHandler
    {
        private ICompanyRepository _companyRepository;
        public GetPriceHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<string> GetPriceAsync(string symbol)
        {
            var quote = await _companyRepository.GetQuoteBySymbolAsync(symbol);
            return CurrencyFormater.FormatCurrency(quote.C.Amount, quote.C.ActualCurrency);
        }
    }
}
