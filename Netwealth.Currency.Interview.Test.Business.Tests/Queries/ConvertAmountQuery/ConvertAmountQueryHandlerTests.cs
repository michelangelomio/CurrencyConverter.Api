using Netwealth.Currency.Interview.Test.Business.Queries.ConvertAmountQuery;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Netwealth.Currency.Interview.Test.Business.Tests.Queries.ConvertAmountQuery
{
    public class ConvertAmountQueryHandlerTests
    {
        [Test]
        public async Task ConvertAmountQueryHandler_SourceAndDestinationAreBaseCurrencyEUR_ReturnTheSameAmount()
        {
            //Arrange
            var query = new Business.Queries.ConvertAmountQuery.ConvertAmountQuery
            {
                Rates = new Dictionary<string, double> { { "EUR", 2 }, { "USD", 3 }, { "GBP", 4 } },
                FromCurrency = "EUR",
                ToCurrency = "EUR",
                Amount = 15
            };

            var handler = new ConvertAmountQueryHandler();

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.AreEqual(result, 15);
        }

        [Test]
        public async Task ConvertAmountQueryHandler_SourceIsBaseCurrencyEUR_ReturnTheAmountTimesDestinationCurrency()
        {
            //Arrange
            var query = new Business.Queries.ConvertAmountQuery.ConvertAmountQuery
            {
                Rates = new Dictionary<string, double> { { "EUR", 2 }, { "USD", 3 }, { "GBP", 4 } },
                FromCurrency = "EUR",
                ToCurrency = "USD",
                Amount = 15
            };

            var handler = new ConvertAmountQueryHandler();

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.AreEqual(result, 45);
        }

        [Test]
        public async Task ConvertAmountQueryHandler_DestinationIsBaseCurrencyEUR_ReturnTheAmountDividedByDestinationCurrency()
        {
            //Arrange
            var query = new Business.Queries.ConvertAmountQuery.ConvertAmountQuery
            {
                Rates = new Dictionary<string, double> { { "EUR", 2 }, { "USD", 5 }, { "GBP", 6 } },
                FromCurrency = "USD",
                ToCurrency = "EUR",
                Amount = 15
            };

            var handler = new ConvertAmountQueryHandler();

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.AreEqual(result, 3);
        }

        [Test]
        public async Task ConvertAmountQueryHandler_SourceAndDestinationAreNotBaseCurrencyEUR_ReturnTheAmountDividedByDestinationCurrency()
        {
            //Arrange
            var query = new Business.Queries.ConvertAmountQuery.ConvertAmountQuery
            {
                Rates = new Dictionary<string, double> { { "EUR", 2 }, { "USD", 3 }, { "GBP", 5 } },
                FromCurrency = "GBP",
                ToCurrency = "USD",
                Amount = 15
            };

            var handler = new ConvertAmountQueryHandler();

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            Assert.AreEqual(result, 9);
        }
    }
}
