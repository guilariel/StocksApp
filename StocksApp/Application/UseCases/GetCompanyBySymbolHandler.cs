using StocksApp.Domain.Entities;
using StocksApp.Domain.Repositorys;

namespace StocksApp.Application.UseCases
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
