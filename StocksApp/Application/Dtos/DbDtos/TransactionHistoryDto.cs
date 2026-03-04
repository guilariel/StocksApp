namespace StocksApp.Application.Dtos.DbDtos
{
    public class TransactionHistoryDto
    {
        public string owner_id { get; set; }
        public string stock_id { get; set; }
        public double amount { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public DateTime transaction_date { get; set; }

        public TransactionHistoryDto(string Owner_id, string Stock_id, double Amount, double Price, string Currency)
        {
            owner_id = Owner_id;
            stock_id = Stock_id;
            amount = Amount;
            price = Price;
            currency = Currency;
            transaction_date = DateTime.Now;
        }
    }
}
