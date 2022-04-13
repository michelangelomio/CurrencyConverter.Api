using FluentValidation;
using Netwealth.Currency.Interview.Test.Business.Clients.Interfaces;
using Netwealth.Currency.Interview.Test.Shared;
using Netwealth.Currency.Interview.Test.Shared.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Netwealth.Currency.Interview.Test.Business.Queries.ConvertAmountQuery
{
    public class ConvertAmountQueryValidator : AbstractValidator<ConvertAmountQuery>
    {
        private readonly IHttpClientBase _httpClientBase;
        private string HttpCallErrorMessage = string.Empty;

        public ConvertAmountQueryValidator(
            IHttpClientBase httpClientBase)
        {
            _httpClientBase = httpClientBase;

            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Amount)
                .Must(AmountMustBePositiveNumber).WithMessage(nameof(AmountMustBePositiveNumber));

            RuleFor(x => x.FromCurrency)
                .NotEmpty();

            RuleFor(x => x.ToCurrency)
                .NotEmpty();

            RuleFor(x => x)
                .MustAsync(WhenModelIsValidThenCallHttpClientToGetRates)
                .WithMessage("Http Call to get rates failed")
                .Must(GetRatesResponseIsNull)
                .WithMessage(nameof(GetRatesResponseIsNull));

            RuleFor(x => x.FromCurrency)
                .Must(MustExistsInCurrenciesWhiteList)
                .WithMessage(x => $"FromCurrency => {x.FromCurrency} {nameof(MustExistsInCurrenciesWhiteList)}");

            RuleFor(x => x.ToCurrency)
                .Must(MustExistsInCurrenciesWhiteList)
                .WithMessage(x => $"ToCurrency => {x.ToCurrency} {nameof(MustExistsInCurrenciesWhiteList)}");
        }


        private bool AmountMustBePositiveNumber(double amount)
        {
            return amount > 0;
        }

        private async Task<bool> WhenModelIsValidThenCallHttpClientToGetRates(ConvertAmountQuery query, CancellationToken cancellationToken)
        {
            try
            {
                var url = Environment.GetEnvironmentVariable(AppSettingSections.GetRatesApiUrl);
                var accessKey = Environment.GetEnvironmentVariable(AppSettingSections.GetRatesApiAccessKey);

                var response = await _httpClientBase.GetAsync<ConvertAmountQueryResponse>(url, accessKey);

                query.Rates = response.Rates;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool GetRatesResponseIsNull(ConvertAmountQuery query)
        {
            return query.Rates.Any();
        }

        private bool MustExistsInCurrenciesWhiteList(string currency)
        {
            var currencies = Enum.GetValues(typeof(CurrenciesWhiteList)).Cast<CurrenciesWhiteList>().ToList();

            return currencies.Any(x => x.ToString().Equals(currency.ToUpper()));
        }
    }
}
