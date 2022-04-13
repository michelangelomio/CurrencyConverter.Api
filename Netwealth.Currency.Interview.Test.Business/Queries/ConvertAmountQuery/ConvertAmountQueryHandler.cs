using MediatR;
using Netwealth.Currency.Interview.Test.Shared.Enums;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Netwealth.Currency.Interview.Test.Business.Queries.ConvertAmountQuery
{
    public class ConvertAmountQueryHandler : IRequestHandler<ConvertAmountQuery, double>
    {
        public async Task<double> Handle(ConvertAmountQuery request, CancellationToken cancellationToken)
        {
            var sourceCurrencyIsEuro = request.FromCurrency.ToUpper().Equals(CurrenciesWhiteList.EUR.ToString());
            var destinationCurrencyIsEuro = request.ToCurrency.ToUpper().Equals(CurrenciesWhiteList.EUR.ToString());

            var sourceRate = request.Rates.FirstOrDefault(x => x.Key.ToUpper().Equals(request.FromCurrency.ToUpper())).Value;
            var destinationRate = request.Rates.FirstOrDefault(x => x.Key.ToUpper().Equals(request.ToCurrency.ToUpper())).Value;

            if (sourceCurrencyIsEuro && destinationCurrencyIsEuro)
                return request.Amount;

            if (sourceCurrencyIsEuro)
                return request.Amount * destinationRate;

            if (destinationCurrencyIsEuro)
                return request.Amount / sourceRate;

            return (request.Amount / sourceRate) * destinationRate;
        }
    }
}
