using Microsoft.Extensions.Logging;
using StocksApp.Domain.Repositorys;
using StocksApp.Domain.ValueObjects;
using StocksApp.Infrastructure.ExternalServices;
using RabbitMQAndGenericRepository.RabbitMq;
using StocksApp.Application.Dtos.RepositoryDtos;
using StocksApp.Domain.Entities.RepositoryEntites;
namespace StocksApp.Application.UseCases.RepositoryUseCases
{
    public class AddCompanyHandler
    {
        private readonly ICompanyRepository _repository;
        private readonly FinnhubSymbolClient _finnhubSymbolClient;
        private readonly FinnhubQuoteClient _finnhubQuoteClient;
        private readonly RabbitMessageService rabbitMessageService;
        public AddCompanyHandler(
            ICompanyRepository repository,
            FinnhubSymbolClient finnhubSymbolClient,
            FinnhubQuoteClient finnhubQuoteClient,
            RabbitMessageService rabbitmessageservice
            )
        {
            _repository = repository;
            _finnhubSymbolClient = finnhubSymbolClient;
            _finnhubQuoteClient = finnhubQuoteClient;
            rabbitMessageService = rabbitmessageservice;
        }

        public async Task AddCompanyAsync(string symbol, string exchange)
        {

            List<CompanyInfo> companyInfos = await AddCompanyAsyncHelper(symbol, exchange);


            foreach (CompanyInfo companyInfo in companyInfos)
            {
                bool exists = await _repository.ContainsEntityAsync(companyInfo.Key);

                if (exists)
                {
                    await _repository.UpdateAsync(companyInfo);
                }
                else
                {
                    await _repository.AddAsync(companyInfo);
                }
//                await rabbitMessageService.SendMessage<CompanyInfo>(companyInfo);
            }
        }

        private async Task<List<CompanyInfo>> AddCompanyAsyncHelper(string symbol, string exchange)
        {

            List<CompanyInfo> companyInfos = new List<CompanyInfo>();
            IEnumerable<StockSymbolsDto> getSymbol = await _finnhubSymbolClient.GetSymbolsAsync(symbol, exchange);

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

            return companyInfos;
        }
    }
}