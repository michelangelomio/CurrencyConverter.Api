using AutoFixture;
using System;
using System.Linq;

namespace Netwealth.Currency.Interview.Test.Service.Shared.AutoDomainData
{
    internal static class FuncIFixtureFactory
    {
        internal static Func<IFixture> Build()
        {
            return delegate
            {
                IFixture fixture = new Fixture().Customize(new DomainCustomization());
                fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(delegate (ThrowingRecursionBehavior b)
                {
                    fixture.Behaviors.Remove(b);
                });
                fixture.Behaviors.Add(new OmitOnRecursionBehavior());
                return fixture;
            };
        }
    }
}
