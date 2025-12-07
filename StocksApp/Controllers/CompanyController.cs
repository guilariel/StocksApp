using Microsoft.AspNetCore.Mvc;
using RabbitMQAndGenericRepository.RabbitMq;
using StocksApp.Application.Dtos.RepositoryDtos;
using StocksApp.Application.UseCases.RepositoryUseCases;
using StocksApp.Domain.Entities.RepositoryEntites;

namespace StocksApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        [HttpPost("AddCompany")]
        public async Task<ActionResult> AddCompany([FromServices] AddCompanyHandler addCompanyHandler, [FromBody] AddCompanyDto addCompanyDto)
        {
            if (string.IsNullOrWhiteSpace(addCompanyDto.Symbol) || string.IsNullOrWhiteSpace(addCompanyDto.Exchange))
            {
                return BadRequest("Key and exchange must not be null or empty.");
            }
            await addCompanyHandler.AddCompanyAsync(addCompanyDto.Symbol, addCompanyDto.Exchange);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCompany([FromServices] DeleteCompanyHandler deleteCompanyHandler, [FromBody] SymbolDto Key)
        {
            if (string.IsNullOrWhiteSpace(Key.Symbol))
            {
                return BadRequest("Key must not be empty.");
            }

            await deleteCompanyHandler.DeleteAsync(Key);
            return NoContent();
        }
        [HttpGet("all")]
        public async Task<ActionResult<List<CompanyInfo>>> GetAllCompanies([FromServices] GetAllCompaniesHandler getAllCompaniesHandler)
        {
            var companies = await getAllCompaniesHandler.GetCompanysAsync();
            return Ok(companies);
        }
        [HttpGet("{Key}")]
        public async Task<ActionResult<CompanyInfo>> GetCompany([FromServices] GetCompanyBySymbolHandler getCompanyBySymbolHandler, string Key)
        {
            if (string.IsNullOrWhiteSpace(Key))
                return BadRequest("Key must not be null or empty.");

            var company = await getCompanyBySymbolHandler.GetCompanyInfoAsync(Key);
            return Ok(company);
        }
    }
}
