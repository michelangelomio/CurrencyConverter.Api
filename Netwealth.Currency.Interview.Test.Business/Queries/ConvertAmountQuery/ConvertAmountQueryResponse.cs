using System.Collections.Generic;

namespace Netwealth.Currency.Interview.Test.Business.Queries.ConvertAmountQuery
{
    public class ConvertAmountQueryResponse
    {
        public ConvertAmountQueryResponse()
        {
            Rates = new Dictionary<string, double>();
        }

        public Dictionary<string, double> Rates { get; set; }
    }
}
