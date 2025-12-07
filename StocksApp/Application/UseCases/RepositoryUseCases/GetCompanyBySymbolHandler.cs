using StocksApp.Domain.Entities.RepositoryEntites;
using StocksApp.Domain.Repositorys;

namespace StocksApp.Application.UseCases.RepositoryUseCases
{
    public class GetCompanyBySymbolHandler
    {
        private ICompanyRepository _companyRepository;
        public GetCompanyBySymbolHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<CompanyInfo> GetCompanyInfoAsync(string symbol)
        {
            return await _companyRepository.GetByKeyAsync(symbol);
        }
    }
}
