using StocksApp.Application.Dtos.RepositoryDtos;
using StocksApp.Domain.Entities;
using StocksApp.Domain.Repositorys;

namespace StocksApp.Application.UseCases.RepositoryUseCases
{
    public class DeleteCompanyHandler
    {
        private ICompanyRepository _companyRepository;
        public DeleteCompanyHandler(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }
        public async Task DeleteAsync(SymbolDto symbolDto)
        {
            await _companyRepository.DeleteAsync(symbolDto.Symbol);
        }
    }
}
