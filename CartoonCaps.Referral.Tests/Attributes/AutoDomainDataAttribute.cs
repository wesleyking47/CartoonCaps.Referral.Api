using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace CartoonCaps.Referral.Tests.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AutoDomainDataAttribute : AutoDataAttribute
{
  public AutoDomainDataAttribute()
    : base(() => new Fixture()
    .Customize(new AutoMoqCustomization()))
  {
  }
}