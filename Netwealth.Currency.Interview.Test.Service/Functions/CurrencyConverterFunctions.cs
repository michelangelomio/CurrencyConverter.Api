using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Netwealth.Currency.Interview.Test.Business.Queries.ConvertAmountQuery;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace Netwealth.Currency.Interview.Test.Service
{
    public class CurrencyConverterFunctions
    {
        private readonly IMediator _mediator;

        public CurrencyConverterFunctions(
            IMediator mediator)
        {
            _mediator = mediator;
        }

        [FunctionName("ConvertAmount")]
        public async Task<IActionResult> ConvertAmount(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Convert/{fromCurrency}/{toCurrency}/{amount}")] HttpRequest req,
            string fromCurrency,
            string toCurrency,
            double amount)
        {
            try
            {
                var query = new ConvertAmountQuery
                {
                    FromCurrency = fromCurrency,
                    ToCurrency = toCurrency,
                    Amount = amount
                };

                var response = await _mediator.Send(query).ConfigureAwait(false);

                return new OkObjectResult(response);
            }
            catch (ValidationException ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ConvertAmount function failed. Error message: {ex.Message}");
                return new InternalServerErrorResult();
            }
        }
    }
}
