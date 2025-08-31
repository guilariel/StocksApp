using StocksApp.Application.Dtos;
using StocksApp.Domain.ValueObjects;
using StocksApp.Infrastructure.Utilities;
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
                throw new ArgumentException("Key is required.");
            try
            {
                var apiWrapper = new ApiWrapper(maxRetries: 5, delayMs: 1000, limitOfrequestPerSecond: 3);

                var dto = await apiWrapper.ExecuteAsync(async () =>
                {
                    var response = await _httpClient.GetAsync(
                        $"https://finnhub.io/api/v1/quote?symbol={symbol}&token={_apiKey}");

                    response.EnsureSuccessStatusCode();

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    return await response.Content.ReadFromJsonAsync<StockQuoteDto>(options);
                });

                if (dto is null)
                    throw new InvalidOperationException("Failed to deserialize the quote response.");

                return new Quote(dto.C, dto.H, dto.L, dto.O, dto.Pc, dto.T);
            }
            catch (HttpRequestException ex)
            {
                throw new HttpRequestException("Error fetching quote", ex);
            }
            catch (JsonException ex)
            {
                throw new ArgumentException($"Error in Json", ex);
            }

        }
    }
}

