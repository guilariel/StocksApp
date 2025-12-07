namespace StocksApp.Application.Dtos.RepositoryDtos
{
    public class AddCompanyDto
    {
        public string Symbol { get; set; }
        public string Exchange { get; set; }

        public static AddCompanyDto FromEntity(string symbol, string exchange)
        {
            return new AddCompanyDto { Symbol = symbol, Exchange = exchange };
        }
    }
}
