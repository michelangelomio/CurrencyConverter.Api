using Moq;
using Netwealth.Currency.Interview.Test.Business.Clients.Interfaces;
using Netwealth.Currency.Interview.Test.Business.Queries.ConvertAmountQuery;
using Netwealth.Currency.Interview.Test.Service.Shared.AutoDomainData;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Netwealth.Currency.Interview.Test.Business.Tests
{
    public class ConvertAmountQueryValidatorTests
    {
        [Test]
        [AutoDomainData]
        public void When_Successfull_ModelIsValid(
            Mock<IHttpClientBase> mockHttpClientBase)
        {
            //Arrange
            var query = new ConvertAmountQuery
            {
                FromCurrency = "EUR",
                ToCurrency = "USD",
                Amount = 1
            };
            var response = new ConvertAmountQueryResponse
            {
                Rates = new Dictionary<string, double> { { "EUR", 1 }, { "USD", 2 } }
            };

            mockHttpClientBase
                .Setup(x => x.GetAsync<ConvertAmountQueryResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(response);

            var validator = new ConvertAmountQueryValidator(mockHttpClientBase.Object);

            //Act
            var result = validator.Validate(query);

            //Assert
            Assert.IsTrue(result.IsValid);
        }

        [Test]
        [AutoDomainData]
        public void When_ModelNotValid_FromCurrencyIsEmpty(
            Mock<IHttpClientBase> mockHttpClientBase)
        {
            //Arrange
            var query = new ConvertAmountQuery { Amount = 1, FromCurrency = "" };
            var validator = new ConvertAmountQueryValidator(mockHttpClientBase.Object);

            //Act
            var result = validator.Validate(query);

            //Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].ToString().Contains("'From Currency' must not be empty."));
        }

        [Test]
        [AutoDomainData]
        public void When_ModelNotValid_ToCurrencyIsEmpty(
            Mock<IHttpClientBase> mockHttpClientBase)
        {
            //Arrange
            var query = new ConvertAmountQuery { Amount = 1, FromCurrency = "USD", ToCurrency = "" };
            var validator = new ConvertAmountQueryValidator(mockHttpClientBase.Object);

            //Act
            var result = validator.Validate(query);

            //Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].ToString().Contains("'To Currency' must not be empty."));
        }

        [Test]
        [AutoDomainData]
        public void When_ModelNotValid_FromCurrencyMustExistsInCurrenciesWhiteList(
            Mock<IHttpClientBase> mockHttpClientBase)
        {
            //Arrange
            var query = new ConvertAmountQuery { Amount = 1, FromCurrency = "AAA", ToCurrency = "USD" };
            var response = new ConvertAmountQueryResponse
            {
                Rates = new Dictionary<string, double> { { "EUR", 1 }, { "USD", 2 } }
            };

            mockHttpClientBase
                .Setup(x => x.GetAsync<ConvertAmountQueryResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(response);

            var validator = new ConvertAmountQueryValidator(mockHttpClientBase.Object);

            //Act
            var result = validator.Validate(query);

            //Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].ToString().Contains("FromCurrency => AAA MustExistsInCurrenciesWhiteList"));
        }

        [Test]
        [AutoDomainData]
        public void When_ModelNotValid_ToCurrencyMustExistsInCurrenciesWhiteList(
            Mock<IHttpClientBase> mockHttpClientBase)
        {
            //Arrange
            var query = new ConvertAmountQuery { Amount = 1, FromCurrency = "USD", ToCurrency = "AAA" };
            var response = new ConvertAmountQueryResponse
            {
                Rates = new Dictionary<string, double> { { "EUR", 1 }, { "USD", 2 } }
            };

            mockHttpClientBase
                .Setup(x => x.GetAsync<ConvertAmountQueryResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(response);

            var validator = new ConvertAmountQueryValidator(mockHttpClientBase.Object);

            //Act
            var result = validator.Validate(query);

            //Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].ToString().Contains("ToCurrency => AAA MustExistsInCurrenciesWhiteList"));
        }

        [Test]
        [AutoDomainData]
        public void When_ModelNotValid_AmountMustBePositiveNumber(
            Mock<IHttpClientBase> mockHttpClientBase)
        {
            //Arrange
            var query = new ConvertAmountQuery { Amount = -1 };
            var validator = new ConvertAmountQueryValidator(mockHttpClientBase.Object);

            //Act
            var result = validator.Validate(query);

            //Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].ToString().Contains("AmountMustBePositiveNumber"));
        }

        [Test]
        [AutoDomainData]
        public void When_HttpCallFailed_ReturnErrorMessage(
            Mock<IHttpClientBase> mockHttpClientBase,
            Exception exception)
        {
            //Arrange
            var query = new ConvertAmountQuery
            {
                FromCurrency = "EUR",
                ToCurrency = "USD",
                Amount = 1
            };

            mockHttpClientBase
                .Setup(x => x.GetAsync<ConvertAmountQueryResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(exception);

            var validator = new ConvertAmountQueryValidator(mockHttpClientBase.Object);

            //Act
            var result = validator.Validate(query);

            //Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].ToString().Contains("Http Call to get rates failed"));
        }


        [Test]
        [AutoDomainData]
        public void When_HttpCallReturnedZeroRates_ReturnErrorMessage(
            Mock<IHttpClientBase> mockHttpClientBase)
        {
            //Arrange
            var query = new ConvertAmountQuery
            {
                FromCurrency = "EUR",
                ToCurrency = "USD",
                Amount = 1
            };
            var response = new ConvertAmountQueryResponse();

            mockHttpClientBase
                .Setup(x => x.GetAsync<ConvertAmountQueryResponse>(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(response);

            var validator = new ConvertAmountQueryValidator(mockHttpClientBase.Object);

            //Act
            var result = validator.Validate(query);

            //Assert
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors[0].ToString().Contains("GetRatesResponseIsNull"));
        }
    }
}
