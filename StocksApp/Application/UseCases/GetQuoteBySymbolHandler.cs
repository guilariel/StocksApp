using StocksApp.Domain.ValueObjects;
using StocksApp.Domain.Repositorys;

namespace StocksApp.Application.UseCases
{
    public class GetQuoteBySymbolHandler
    {
        private ICompanyRepository _companyRepository;
        public GetQuoteBySymbolHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<Quote> GetQuoteInfoAsync(string symbol)
        {
            return await _companyRepository.GetQuoteBySymbolAsync(symbol);
        }
    }
}
