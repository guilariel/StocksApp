using StocksApp.Application.Dtos.RepositoryDtos;
using StocksApp.Domain.Entities.RepositoryEntites;
using StocksApp.Domain.Repositorys;
using StocksApp.Domain.ValueObjects;
using StocksApp.Infrastructure.ExternalServices;

namespace StocksApp.Application.UseCases.RepositoryUseCases
{
    public class UpdateCompanys
    {
        public ICompanyRepository _companyRepository;
        private readonly FinnhubSymbolClient _finnhubSymbolClient;
        private readonly FinnhubQuoteClient _finnhubQuoteClient;
        public UpdateCompanys(ICompanyRepository companyRepository, FinnhubSymbolClient finnhubSymbolClient, FinnhubQuoteClient finnhubQuoteClient)
        {
            _companyRepository = companyRepository;
            _finnhubQuoteClient = finnhubQuoteClient;
            _finnhubSymbolClient = finnhubSymbolClient;
        }
        public async Task UpdateAsync()
        {
            IEnumerable<CompanyInfo> companyInfos = await UpdateCompanyAsyncHelper();
            foreach (CompanyInfo companyInfo in companyInfos)
            {
                await _companyRepository.UpdateAsync(companyInfo);
            }
        }
        private async Task<List<CompanyInfo>> UpdateCompanyAsyncHelper()
        {
            List<CompanyInfo> companyInfos = new List<CompanyInfo>();
            foreach (CompanyInfo company in await _companyRepository.GetAllAsync())
            {
                IEnumerable<StockSymbolsDto> getSymbol = await _finnhubSymbolClient.GetSymbolsAsync(company.Key, company.Quote.L.ActualCurrency);

                if (getSymbol == null || !getSymbol.Any())
                {
                    return companyInfos;
                }

                foreach (StockSymbolsDto stockSymbols in getSymbol)
                {
                    Quote quote = await _finnhubQuoteClient.GetQuoteAsync(stockSymbols.Symbol);

                    CompanyInfo companyInfo = new CompanyInfo(
                        stockSymbols.Description,
                        stockSymbols.DisplaySymbol,
                        stockSymbols.Symbol,
                        stockSymbols.Type,
                        quote
                    );

                    companyInfos.Add(companyInfo);
                }
            }
                return companyInfos;
        }
    }
}
