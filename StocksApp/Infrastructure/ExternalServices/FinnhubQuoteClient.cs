using StocksApp.Application.Dtos.RepositoryDtos;
using StocksApp.Domain.ValueObjects;
using System.Net.Http.Json;
using System.Text.Json;

namespace StocksApp.Infrastructure.ExternalServices
{
    public class FinnhubQuoteClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public FinnhubQuoteClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["Finnhub:ApiKey"];
            Console.WriteLine("DEBUG - API Key cargada: " + (_apiKey ?? "NULL"));
        }

        public async Task<Quote> GetQuoteAsync(string symbol)
        {
            if (string.IsNullOrEmpty(symbol))
                throw new ArgumentException("Symbol is required.");

            try
            {
                // Construimos la URL usando la API Key cargada desde la configuración
                var url = $"https://finnhub.io/api/v1/quote?symbol={symbol}&token=d0ovj61r01qr8ds0ibp0d0ovj61r01qr8ds0ibpg";

                // Hacemos la llamada HTTP directa
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Leemos el JSON directamente en nuestro DTO
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dto = await response.Content.ReadFromJsonAsync<StockQuoteDto>(options);

                if (dto is null)
                    throw new InvalidOperationException("Failed to deserialize the quote response.");

                // Convertimos el DTO a nuestra entidad de dominio
                return new Quote(dto.C, dto.H, dto.L, dto.O, dto.Pc, dto.T);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error fetching quote", ex);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException("Error parsing JSON response", ex);
            }
        }
    }
}