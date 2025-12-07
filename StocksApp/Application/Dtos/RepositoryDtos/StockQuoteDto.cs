using System.Data.SqlTypes;
using System.Text.Json.Serialization;

namespace StocksApp.Application.Dtos.RepositoryDtos
{
    public class StockQuoteDto
    {
        [JsonPropertyName("c")]
        public decimal C { get; set; }
        [JsonPropertyName("h")]
        public decimal H { get; set; }
        [JsonPropertyName("l")]
        public decimal L { get; set; }
        [JsonPropertyName("o")]
        public decimal O { get; set; }
        [JsonPropertyName("pc")]
        public decimal Pc { get; set; }
        [JsonPropertyName("t")]
        public long T { get; set; }
    }
}
