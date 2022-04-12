using FluentValidation;
using Netwealth.Currency.Interview.Test.Shared.Enums;
using System;
using System.Linq;

namespace Netwealth.Currency.Interview.Test.Business.Queries.ConvertAmountQuery
{
    public class ConvertAmountQueryValidator : AbstractValidator<ConvertAmountQuery>
    {
        public ConvertAmountQueryValidator()
        {
            RuleFor(x => x.Amount)
                .NotEmpty();

            RuleFor(x => x.FromCurrency)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.FromCurrency)
                .Must(ExistsInCurrenciesWhiteList)
                .WithMessage(nameof(ExistsInCurrenciesWhiteList));

            RuleFor(x => x.ToCurrency)
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.ToCurrency)
                .Must(ExistsInCurrenciesWhiteList)
                .WithMessage(nameof(ExistsInCurrenciesWhiteList));
        }

        private bool ExistsInCurrenciesWhiteList(string currency)
        {
            var currencies = Enum.GetValues(typeof(CurrenciesWhiteList)).Cast<CurrenciesWhiteList>().ToList();

            return currencies.Any(x => x.ToString().Equals(currency.ToUpper()));
        }
    }
}
