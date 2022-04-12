using DeeSet.JobHub.Merchandisers.Business.PipelineBehaviors;
using FluentValidation;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Netwealth.Currency.Interview.Test.Business.Clients;
using Netwealth.Currency.Interview.Test.Business.Clients.Interfaces;
using Netwealth.Currency.Interview.Test.Business.Queries.ConvertAmountQuery;
using Netwealth.Currency.Interview.Test.Service.Functions;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace Netwealth.Currency.Interview.Test.Service.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var currentAssembly = AppDomain.CurrentDomain.GetAssemblies();

            builder.Services.AddMediatR(currentAssembly);
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            builder.Services.AddValidatorsFromAssembly(typeof(ConvertAmountQueryValidator).Assembly);
            builder.Services.AddHttpClient<IHttpClientBase, HttpClientBase>();
        }
    }
}