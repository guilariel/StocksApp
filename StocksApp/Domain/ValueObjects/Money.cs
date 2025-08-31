using System.Runtime.CompilerServices;

namespace StocksApp.Domain.ValueObjects
{
    public class Money
    {
        public decimal Amount { get; }
        public string ActualCurrency { get; }
        public Money(decimal amount, string currency) 
        {
            Amount = amount;
            if (currency.ContainsCoin())
            {
                ActualCurrency = currency;
            }
            else
            {
                throw new ArgumentException("moneda invalida o no existente");
            }
        }
        public static bool operator ==(Money? left, Money? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }
        public static bool operator !=(Money? left, Money? right)
        {
            return !(left == right);
        }
    }
    public static class MoneyExtensions
    {
        public static bool EqualMoney(this Money money, Money other)
        {
            return money.Amount == other.Amount && money.ActualCurrency == other.ActualCurrency;
        }
        public static Money ChangeCurrency(this Money money, string currency)
        {
            if (currency.ContainsCoin())
            {
                return new Money(money.Amount * money.ActualCurrency.GetExchangeRate() / currency.GetExchangeRate(), currency);
            }
            else
            {
                throw new ArgumentException("moneda invalida o no existente");
            }
        }
        public static Money AddMoney(this Money money, decimal amount)
        {
            return new Money(money.Amount + amount, money.ActualCurrency);
        }
        public static Money RemoveAmount(this Money money, decimal amount)
        {
            return new Money(money.Amount - amount, money.ActualCurrency);
        }
        public static bool IsntEqual(this Money money, Money other)
        {
            return money.Amount != other.Amount || money.ActualCurrency != other.ActualCurrency;
        }
    }
}






/*        public decimal Amount { get; }
        public string ActualCurrency { get; }
        public Money(decimal amount, string currency) 
        {
            Amount = amount;
            if (currency.ContainsCoin())
            {
                ActualCurrency = currency;
            }
            else
            {
                throw new ArgumentException("moneda invalida o no existente");
            }
        }
        public override bool Equals(object obj)
        {
            if (obj is Money other)
                return Amount == other.Amount && ActualCurrency == other.ActualCurrency;
            return false;
        }
        public static bool operator ==(Money? left, Money? right)
        {
            if (ReferenceEquals(left, right)) return true;
            if (left is null || right is null) return false;
            return left.Equals(right);
        }
        public static bool operator !=(Money? left, Money? right)
        {
            return !(left == right);
        }
        public Money ChangeCurrency(string currency)
        {
            if (currency.ContainsCoin())
            {
                return new Money(Amount * ActualCurrency.GetExchangeRate() / currency.GetExchangeRate(), currency);
            }
            else
            {
                throw new ArgumentException("moneda invalida o no existente");
            }
        }
        public Money AddMoney(decimal amount)
        {
            return new Money(Amount + amount, ActualCurrency);
        }
        public Money RemoveAmount(decimal amount)
        {
            if(Amount - amount >= 0)
               return new Money(Amount - amount, ActualCurrency);
            throw new ArgumentException("fondos insuficientes");
        }*/