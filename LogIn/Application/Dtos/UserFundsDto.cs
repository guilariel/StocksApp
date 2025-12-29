namespace LogIn.Application.Dtos
{
    public class UserFundsDto
    {
        public string UserName { get; set; }
        public double Funds { get; set; }
        public string Currency {  get; set; }
        public UserFundsDto(string userName, double funds, string currency) 
        {
            UserName = userName;
            Funds = funds;
            Currency = currency;
        }
    }
}
