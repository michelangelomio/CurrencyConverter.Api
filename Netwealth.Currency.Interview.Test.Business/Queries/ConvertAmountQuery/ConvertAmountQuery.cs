using MediatR;

namespace Netwealth.Currency.Interview.Test.Business.Queries.ConvertAmountQuery
{
    public class ConvertAmountQuery : IRequest<double>
    {
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public double Amount { get; set; }
    }
}
