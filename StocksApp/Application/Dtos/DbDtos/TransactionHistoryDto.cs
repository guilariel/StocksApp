namespace StocksApp.Application.Dtos.DbDtos
{
    public class TransactionHistoryDto
    {
        public int owner_id { get; set; }
        public int stock_id { get; set; }
        public int amount { get; set; }
        public double price { get; set; }
        public string currency { get; set; }
        public DateTime transaction_date { get; set; }

        public TransactionHistoryDto(int Owner_id, int Stock_id, int Amount, double Price, string Currency)
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
