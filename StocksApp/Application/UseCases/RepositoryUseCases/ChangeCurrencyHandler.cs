using StocksApp.Domain.Repositorys;
using StocksApp.Domain.ValueObjects;

namespace StocksApp.Application.UseCases.RepositoryUseCases
{
    public class ChangeCurrencyHandler
    {
        private ICompanyRepository _companyRepository;
        public ChangeCurrencyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task ChangeCurrencyAsync(string symbol, string currency)
        {
            var company = await _companyRepository.GetByKeyAsync(symbol);

            var updatedQuote = company.Quote.ChangeCurrency(currency);
            var updatedCompany = company.WithQuote(updatedQuote);

            await _companyRepository.UpdateAsync(updatedCompany);
        }
    }
}
