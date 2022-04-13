using AutoFixture.NUnit3;

namespace Netwealth.Currency.Interview.Test.Service.Shared.AutoDomainData
{
    public class AutoDomainDataAttribute : AutoDataAttribute
    {
        public AutoDomainDataAttribute()
            : base(FuncIFixtureFactory.Build())
        {
        }
    }
}
