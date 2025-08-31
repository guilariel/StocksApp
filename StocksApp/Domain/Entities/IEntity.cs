namespace StocksApp.Domain.Entities
{
    public interface IEntity<T> 
    {
        T Key { get; }
    }
}
