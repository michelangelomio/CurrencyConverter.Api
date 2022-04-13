using AutoFixture;
using AutoFixture.AutoMoq;

namespace Netwealth.Currency.Interview.Test.Service.Shared.AutoDomainData
{
    public class DomainCustomization : CompositeCustomization
    {
        public DomainCustomization()
            : base(new AutoMoqCustomization())
        {
        }
    }
}
