using AutoFixture.NUnit3;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Netwealth.Currency.Interview.Test.Business.Queries.ConvertAmountQuery;
using Netwealth.Currency.Interview.Test.Service.Shared.AutoDomainData;
using NUnit.Framework;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Netwealth.Currency.Interview.Test.Service.Tests
{
    public class CurrencyConverterTests
    {
        [Test]
        [AutoDomainData]
        public async Task When_WhenConvertedSuccessfully_ReturnOkObjectResultWithValue(
            [Frozen] Mock<IMediator> mockMediator,
            CurrencyConverterFunctions currencyConverterFunctions
            )
        {
            //Arrange
            var request = new DefaultHttpContext().Request;

            mockMediator
                .Setup(m => m.Send(It.IsAny<ConvertAmountQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(2);

            //Act
            var result = (OkObjectResult)await currencyConverterFunctions.ConvertAmount(request, "fromCurrency", "toCurrency", 1);

            //Assert
            Assert.AreEqual(result.StatusCode, 200);
            Assert.AreEqual(result.Value, 2);
        }

        [Test]
        [AutoDomainData]
        public async Task When_ValidationException_ReturnBadRequestWithErrorMessage(
            [Frozen] Mock<IMediator> mockMediator,
            CurrencyConverterFunctions currencyConverterFunctions,
            ValidationException exceprion
            )
        {
            //Arrange
            var request = new DefaultHttpContext().Request;

            mockMediator
                .Setup(m => m.Send(It.IsAny<ConvertAmountQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exceprion);

            //Act
            var result = (BadRequestObjectResult)await currencyConverterFunctions.ConvertAmount(request, "fromCurrency", "toCurrency", 1);

            //Assert
            Assert.AreEqual(result.StatusCode, 400);
        }

        [Test]
        [AutoDomainData]
        public async Task When_ValidationException_ReturnInternalServerErrorResult(
           [Frozen] Mock<IMediator> mockMediator,
           CurrencyConverterFunctions currencyConverterFunctions,
           Exception exception
           )
        {
            //Arrange
            var request = new DefaultHttpContext().Request;

            mockMediator
                .Setup(m => m.Send(It.IsAny<ConvertAmountQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception);

            //Act
            var result = (InternalServerErrorResult)await currencyConverterFunctions.ConvertAmount(request, "fromCurrency", "toCurrency", 1);

            //Assert
            Assert.AreEqual(result.StatusCode, 500);
        }
    }
}
