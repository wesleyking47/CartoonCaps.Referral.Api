using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CartoonCaps.Referral.Api.Tests.Attributes.Customizations;

public class ControllerCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customize<ControllerContext>(c => c.OmitAutoProperties());
        fixture.Customize<ControllerActionDescriptor>(c => c.OmitAutoProperties());
        fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
    }
}