using Microsoft.Extensions.Options;
using StocksApp.Application.Dtos.RepositoryDtos;
using StocksApp.Domain.Entities;
using StocksApp.Domain.ValueObjects;
using System.Net.Http.Json;
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
                // Construimos la URL usando la API Key
                var url = $"https://finnhub.io/api/v1/search?q={name}&exchange={exchange}&token=d0ovj61r01qr8ds0ibp0d0ovj61r01qr8ds0ibpg";

                // Llamada HTTP directa
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Leemos el JSON directamente en nuestro DTO
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dto = await response.Content.ReadFromJsonAsync<SymbolResponseDto>(options);

                // Devolvemos la lista de resultados o vacío
                return dto?.Result ?? Enumerable.Empty<StockSymbolsDto>();
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error fetching symbols", ex);
            }
            catch (JsonException ex)
            {
                throw new Exception("Error parsing JSON response", ex);
            }
        }
    }
}
