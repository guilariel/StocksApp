namespace PurchaseStocks.Application.Dtos
{
    public class TransactionDto
    {
        public int owner_id { get; set; }
        public int stock_id { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
    }
}
