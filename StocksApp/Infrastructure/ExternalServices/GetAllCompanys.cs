using Npgsql;
using StocksApp.Application.Dtos;

namespace StocksApp.Infrastructure.ExternalServices
{
    public class GetAllCompanys
    {
        private readonly string _connectionString;
        public GetAllCompanys(string connectionString)
        {
            _connectionString = connectionString;
        }
        public async Task<List<CompanyDbDto>> GetCompanys()
        {
            List<CompanyDbDto> results = new List<CompanyDbDto>(); 
            using var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            using var companyComand = new NpgsqlCommand("SELECT * FROM GetAllCompanies()", connection);
            using var reader = await companyComand.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                CompanyDbDto company = new CompanyDbDto
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1),
                    Description = reader.GetString(2),
                    Category = reader.GetString(3)
                };
                results.Add(company);
            }

            return results;
        }
    }
}
