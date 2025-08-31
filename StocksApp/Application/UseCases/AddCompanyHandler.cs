using Microsoft.Extensions.Logging;
using StocksApp.Application.Dtos;
using StocksApp.Domain.Entities;
using StocksApp.Domain.Repositorys;
using StocksApp.Domain.ValueObjects;
using StocksApp.Infrastructure.ExternalServices;
// ...otros usings...

namespace StocksApp.Application.UseCases
{
    public class AddCompanyHandler
    {
        private readonly ICompanyRepository _repository;
        private readonly FinnhubSymbolClient _finnhubSymbolClient;
        private readonly FinnhubQuoteClient _finnhubQuoteClient;
        private readonly ILogger<AddCompanyHandler> _logger;

        public AddCompanyHandler(
            ICompanyRepository repository,
            FinnhubSymbolClient finnhubSymbolClient,
            FinnhubQuoteClient finnhubQuoteClient,
            ILogger<AddCompanyHandler> logger)
        {
            _repository = repository;
            _finnhubSymbolClient = finnhubSymbolClient;
            _finnhubQuoteClient = finnhubQuoteClient;
            _logger = logger;
        }

        public async Task AddCompanyAsync(string symbol, string exchange)
        {
            _logger.LogDebug("Iniciando AddCompanyAsync para symbol: '{symbol}', exchange: '{exchange}'", symbol, exchange);

            List<CompanyInfo> companyInfos = await AddCompanyAsyncHelper(symbol, exchange);

            _logger.LogDebug("Total de CompanyInfo encontrados: {Count}", companyInfos.Count);

            foreach (CompanyInfo companyInfo in companyInfos)
            {
                _logger.LogDebug("Procesando CompanyInfo: {Key}, {DisplaySymbol}", companyInfo.Key, companyInfo.DisplaySymbol);

                bool exists = await _repository.ContainsEntityAsync(companyInfo.Key);
                _logger.LogDebug("¿Ya existe en repo? {Exists}", exists);

                if (exists)
                {
                    _logger.LogDebug("Actualizando {Key}", companyInfo.Key);
                    await _repository.UpdateAsync(companyInfo);
                }
                else
                {
                    _logger.LogDebug("Agregando {Key}", companyInfo.Key);
                    await _repository.AddAsync(companyInfo);
                }
            }
        }

        private async Task<List<CompanyInfo>> AddCompanyAsyncHelper(string symbol, string exchange)
        {
            _logger.LogDebug("Llamando a FinnhubSymbolClient para exchange '{exchange}' y symbol '{symbol}'", exchange, symbol);

            List<CompanyInfo> companyInfos = new List<CompanyInfo>();
            IEnumerable<StockSymbolsDto> getSymbol = await _finnhubSymbolClient.GetSymbolsAsync(symbol, exchange);

            if (getSymbol == null || !getSymbol.Any())
            {
                _logger.LogError("No se encontraron símbolos para exchange '{exchange}' y symbol '{symbol}'", exchange, symbol);
                return companyInfos;
            }

            _logger.LogDebug("Símbolos obtenidos: {Symbols}", string.Join(", ", getSymbol.Select(s => s.Symbol)));

            foreach (StockSymbolsDto stockSymbols in getSymbol)
            {
                _logger.LogDebug("Obteniendo quote para {Symbol}", stockSymbols.Symbol);

                Quote quote = await _finnhubQuoteClient.GetQuoteAsync(stockSymbols.Symbol);

                if (quote == null)
                {
                    _logger.LogWarning("No se obtuvo quote para {Symbol}", stockSymbols.Symbol);
                }
                else
                {
                    _logger.LogDebug("Quote recibido: Precio actual = {C}, Apertura = {O}", quote.C, quote.O);
                }

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