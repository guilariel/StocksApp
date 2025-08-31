using Microsoft.Extensions.Options;
using StocksApp.Application.Dtos;
using StocksApp.Domain.Entities;
using StocksApp.Domain.ValueObjects;
using StocksApp.Infrastructure.Utilities;
using System.Net.Http;
using System.Text.Json;
namespace StocksApp.Infrastructure.ExternalServices
{
    public class FinnhubSymbolClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        public FinnhubSymbolClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Finnhub:ApiKey"];
            Console.WriteLine("DEBUG - API Key cargada: " + (_apiKey ?? "NULL"));
        }
        public async Task<IEnumerable<StockSymbolsDto>> GetSymbolsAsync(string name, string exchange)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(exchange))
            {
                return Enumerable.Empty<StockSymbolsDto>();
            }
            try
            {
                var ApiWraper = new ApiWrapper(maxRetries: 5, delayMs: 1000, limitOfrequestPerSecond: 3);
                var dto = await ApiWraper.ExecuteAsync(async () =>
                {
                    var response = await _httpClient.GetAsync(
                        $"https://finnhub.io/api/v1/search?q={name}&exchange={exchange}&token={_apiKey}");

                    response.EnsureSuccessStatusCode();

                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    return await response.Content.ReadFromJsonAsync<SymbolResponseDto>(options);
                });

                return dto?.Result ?? Enumerable.Empty<StockSymbolsDto>();
            }
            catch (HttpRequestException ex)
            {
                // Manejo de errores de solicitud HTTP
                throw new HttpRequestException($"Error when get stocks:", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception($"Error:", ex );
            }
        }
    }
}
