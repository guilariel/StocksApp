using StocksApp.Domain.ValueObjects;
using System.Xml.Linq;
using RabbitMQAndGenericRepository.Repositorio;
namespace StocksApp.Domain.Entities.RepositoryEntites
{
    public class CompanyInfo : IEntity<string>
    {
        public string Description { get; } 
        public string DisplaySymbol { get; }
        public string Key {  get; }
        public string Type { get; }
        public Quote Quote { get; }
        public CompanyInfo(string description, string displaySymbol,string symbol, string type, Quote quote)
        {
            Description = description;
            DisplaySymbol = displaySymbol;
            Key = symbol;
            Type = type;
            Quote = quote;
        }
        public CompanyInfo(string description, string displaySymbol, string symbol, string type) 
        {
            Description = description;
            DisplaySymbol = displaySymbol;
            Key = symbol;
            Type = type;
            Quote = null;
        }
        public override bool Equals(object obj)
        {
            if (obj is not CompanyInfo other)
                return false;

            return Key == other.Key;
        }
        public override int GetHashCode()
        {
            return Key.GetHashCode();
        }

        public CompanyInfo WithQuote(Quote newQuote)
        {
            return new CompanyInfo(Description, DisplaySymbol, Key, Type, newQuote);
        }
    }
}
