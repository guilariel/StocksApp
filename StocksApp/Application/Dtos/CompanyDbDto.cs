namespace StocksApp.Application.Dtos
{
    public class CompanyDbDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }

        public static CompanyDbDto FromEntity(int id, string name, string description, string category)
        {
            return new CompanyDbDto { Id = id, Name = name, Description = description, Category = category };
        }
    }
}
