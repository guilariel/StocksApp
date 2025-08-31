using System.Text.Json.Serialization;
namespace StocksApp.Domain.ValueObjects
{
    public class Quote
    {
        public Money C { get; }
        public Money H { get; }
        public Money L { get; }
        public Money O { get; }
        public Money Pc { get; }
        public Money T { get; }
        public Quote(Money c, Money h, Money l, Money o, Money pc, Money t)
        {
            C = c;
            H = h;
            L = l;
            O = o;
            Pc = pc;
            T = t;
        }
        public Quote(decimal c, decimal h, decimal l, decimal o, decimal pc, decimal t) 
        {
            C = new Money(c,"USD");
            H = new Money(h, "USD");
            L = new Money(l, "USD");
            O = new Money(o, "USD");
            Pc = new Money(pc, "USD");
            T = new Money(t, "USD");
        }
    }
    public static class QuoteExtensions
    {
        public static Quote ChangeCurrency(this Quote quote, string currency)
        {
            return new Quote(
                quote.C.ChangeCurrency(currency),
                quote.H.ChangeCurrency(currency),
                quote.L.ChangeCurrency(currency),
                quote.O.ChangeCurrency(currency),
                quote.Pc.ChangeCurrency(currency),
                quote.T.ChangeCurrency(currency)
            );
        }
    }
}
