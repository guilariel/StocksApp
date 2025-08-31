using StocksApp.Domain.Entities;
using StocksApp.Domain.Repositorys;
namespace StocksApp.Application.UseCases
{
    public class GetAllCompaniesHandler
    {
        private ICompanyRepository _companyRepository;
        public GetAllCompaniesHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task<IEnumerable<CompanyInfo>> GetCompanysAsync()
        {
            return await _companyRepository.GetAllAsync();
        }
    }
}
