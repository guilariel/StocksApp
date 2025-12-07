using StocksApp.Domain.ValueObjects;
using RabbitMQAndGenericRepository.Repositorio;
using StocksApp.Domain.Entities.RepositoryEntites;
namespace StocksApp.Domain.Repositorys
{
    public interface ICompanyRepository : IGenericRepository<CompanyInfo, string>
    {
        Task<Quote> GetQuoteBySymbolAsync(string Key);
    }
    public class CompanyRepository : GenericRepository<CompanyInfo, string> , ICompanyRepository
    {
        public Task<Quote> GetQuoteBySymbolAsync(string Key)
        {
            var result = _entities.FirstOrDefault(c => c.Key == Key);
            if (result is null)
                throw new ArgumentException("no se tiene registrada a esa compania");
            return Task.FromResult(result.Quote);
        }
    }
}
//ctrl k + ctrl d para ordenar










/*using StocksApp.Domain.Entities;
using StocksApp.Domain.ValueObjects;
namespace StocksApp.Domain.Repositorys
{
    public interface ICompanyRepository
    {
        Task<CompanyInfo> GetInfoBySymbolAsync(string Key);
        Task<Quote> GetQuoteBySymbolAsync(string Key);
        Task<List<CompanyInfo>> GetCompanyInfoAsync();
        Task AddCompanyAsync(CompanyInfo companyInfo);
        Task DeleteCompanyAsync(string Key);
        Task UpdateCompanyAsync(CompanyInfo companyInfo);
        Task<bool> ContainsCompanyAsync(CompanyInfo companyInfo);
    }
    public class CompanyRepository : ICompanyRepository
    {
        private List<CompanyInfo> _companyInfos { get; set; } = new List<CompanyInfo>();

        public Task<CompanyInfo> GetInfoBySymbolAsync(string Key)
        {
            var result = _companyInfos.FirstOrDefault(c => c.Key == Key);
            if (result is null)
                throw new ArgumentException("no se tiene registrada a esa compania");
            return Task.FromResult(result);
        }

        public Task<Quote> GetQuoteBySymbolAsync(string Key)
        {
            var result = _companyInfos.FirstOrDefault(c => c.Key == Key);
            if (result is null)
                throw new ArgumentException("no se tiene registrada a esa compania");
            return Task.FromResult(result.Quote);
        }

        public Task<List<CompanyInfo>> GetCompanyInfoAsync()
        {
            return Task.FromResult(_companyInfos);
        }

        public Task AddCompanyAsync(CompanyInfo companyInfo)
        {
            if (_companyInfos.Any(c => c.Key == companyInfo.Key))
                throw new ArgumentException("ya se registro esa compania");

            _companyInfos.Add(companyInfo);
            return Task.CompletedTask;
        }

        public Task DeleteCompanyAsync(string Key)
        {
            var removed = _companyInfos.RemoveAll(c => c.Key == Key);
            if (removed == 0)
                throw new ArgumentException("no se tiene registrada a esa compania");

            return Task.CompletedTask;
        }

        public Task UpdateCompanyAsync(CompanyInfo companyInfo)
        {
            for (int i = 0; i < _companyInfos.Count; i++)
            {
                if (_companyInfos[i].Key == companyInfo.Key)
                {
                    _companyInfos[i] = companyInfo;
                    return Task.CompletedTask;
                }
            }
            throw new ArgumentException("no se tiene registrada a esa compania");
        }

        public Task<bool> ContainsCompanyAsync(CompanyInfo companyInfo)
        {
            return Task.FromResult(_companyInfos.Contains(companyInfo));
        }
    }
}

*/