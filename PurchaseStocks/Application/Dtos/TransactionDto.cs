namespace PurchaseStocks.Application.Dtos
{
    public class TransactionDto
    {
        public string owner_id { get; set; }
        public string stock_id { get; set; }
        public double amount { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public DateTime date { get; set; }
        public string type { get; set; }
    }
}
