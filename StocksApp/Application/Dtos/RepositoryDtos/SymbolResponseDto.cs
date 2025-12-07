using System.Text.Json.Serialization;

namespace StocksApp.Application.Dtos.RepositoryDtos
{
    public class SymbolResponseDto
    {
        [JsonPropertyName("count")]
        public int Count { get; set; } = 0;

        [JsonPropertyName("result")]
        public List<StockSymbolsDto> Result { get; set; } = new();
    }
}
