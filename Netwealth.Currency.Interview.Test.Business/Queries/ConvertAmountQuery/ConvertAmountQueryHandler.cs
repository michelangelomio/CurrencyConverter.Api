using MediatR;
using Netwealth.Currency.Interview.Test.Business.Clients.Interfaces;
using Netwealth.Currency.Interview.Test.Shared;
using Netwealth.Currency.Interview.Test.Shared.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Netwealth.Currency.Interview.Test.Business.Queries.ConvertAmountQuery
{
    public class ConvertAmountQueryHandler : IRequestHandler<ConvertAmountQuery, double>
    {
        private readonly IHttpClientBase _clientBase;

        public ConvertAmountQueryHandler(
            IHttpClientBase clientBase)
        {
            _clientBase = clientBase;
        }

        public async Task<double> Handle(ConvertAmountQuery request, CancellationToken cancellationToken)
        {
            var response = await GetRatesFromHttpCall();

            var sourceCurrencyIsEuro = request.FromCurrency.ToUpper().Equals(CurrenciesWhiteList.EUR.ToString());
            var destinationCurrencyIsEuro = request.ToCurrency.ToUpper().Equals(CurrenciesWhiteList.EUR.ToString());

            var sourceRate = response.Rates.FirstOrDefault(x => x.Key.ToUpper().Equals(request.FromCurrency.ToUpper())).Value;
            var destinationRate = response.Rates.FirstOrDefault(x => x.Key.ToUpper().Equals(request.ToCurrency.ToUpper())).Value;

            if (sourceCurrencyIsEuro && destinationCurrencyIsEuro)
                return request.Amount;

            if (sourceCurrencyIsEuro)
                return request.Amount * destinationRate;

            if (destinationCurrencyIsEuro)
                return request.Amount / sourceRate;

            return (request.Amount / sourceRate) * destinationRate;
        }

        private async Task<ConvertAmountQueryResponse> GetRatesFromHttpCall()
        {
            var url = Environment.GetEnvironmentVariable(AppSettingSections.GetRatesApiUrl);
            var accessKey = Environment.GetEnvironmentVariable(AppSettingSections.GetRatesApiAccessKey);

            return await _clientBase.GetAsync<ConvertAmountQueryResponse>(url, accessKey);
        }
    }
}
