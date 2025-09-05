using Npgsql;

namespace StocksApp.Infrastructure.ExternalServices
{
    public class AddOrUpdateToDatabase
    {
        private readonly string _connectionString;
        public AddOrUpdateToDatabase(string connectionString)
        {
            _connectionString = connectionString;
        }
        public void AddOrUpdate()
        {

        }
        public void InsertCompany(string name, string description, string category)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var companyComand = new NpgsqlCommand("CALL insertcompany(@name, @description, @category)", connection);
            companyComand.Parameters.AddWithValue("name", name);
            companyComand.Parameters.AddWithValue("description", description);
            companyComand.Parameters.AddWithValue("category", category);

            companyComand.ExecuteNonQuery();


        }
        public void InsertStock(string symbol, string name, string description, int companyId)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var companyComand = new NpgsqlCommand("CALL insertstock(@symbol, @name, @companyId)", connection);
            companyComand.Parameters.AddWithValue("symbol", symbol);
            companyComand.Parameters.AddWithValue("name", name);
            companyComand.Parameters.AddWithValue("description", description);
            companyComand.Parameters.AddWithValue("companyId", companyId);

            companyComand.ExecuteNonQuery();
        }//datos pasar a entity framework
        //tratar de hcacaer andar rabitmq en la pc
        //tratar de leer mensajes
        public void InsertPrice(int stockId, double price, DateTime date)
        {
            using var connection = new NpgsqlConnection(_connectionString);
            connection.Open();

            using var companyComand = new NpgsqlCommand("CALL insertpricehistory(@stockId, @price, @date)", connection);
            companyComand.Parameters.AddWithValue("stockId", stockId);
            companyComand.Parameters.AddWithValue("price", price);
            companyComand.Parameters.AddWithValue("date", date);

            companyComand.ExecuteNonQuery();
        }
    }
}
