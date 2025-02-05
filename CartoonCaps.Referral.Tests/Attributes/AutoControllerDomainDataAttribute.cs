using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using CartoonCaps.Referral.Api.Tests.Attributes.Customizations;

namespace CartoonCaps.Referral.Api.Tests.Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AutoControllerDomainDataAttribute : AutoDataAttribute
{
  public AutoControllerDomainDataAttribute()
    : base(() => new Fixture()
    .Customize(new AutoMoqCustomization())
    .Customize(new ControllerCustomization()))
  {
  }
}