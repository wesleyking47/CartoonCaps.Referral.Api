using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace CartoonCaps.Referral.Tests.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AutoOmitRecursionDomainDataAttribute : AutoDataAttribute
{
  public AutoOmitRecursionDomainDataAttribute()
    : base(() =>
    {
      var fixture = new Fixture().Customize(new AutoMoqCustomization());

      fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
        .ForEach(b => fixture.Behaviors.Remove(b));

      fixture.Behaviors.Add(new OmitOnRecursionBehavior());

      return fixture;
    })
  {
  }
}