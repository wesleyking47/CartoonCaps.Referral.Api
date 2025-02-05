using CartoonCaps.Referral.Api.Controllers.v1;
using CartoonCaps.Referral.Api.Tests.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Moq;

namespace CartoonCaps.Referral.Api.Tests.Controllers.v1;

public class ErrorControllerTests
{
    [Theory]
    [AutoControllerDomainData]
    public void GivenNonDevelopmentEnvironment_WhenHandleErrorDevelopment_ThenReturnNotFound(
        Mock<IHostEnvironment> hostEnvironmentMock,
        ErrorController controller
    )
    {
        hostEnvironmentMock.Setup(x => x.EnvironmentName).Returns("NotDevelopment");

        var result = controller.HandleErrorDevelopment(hostEnvironmentMock.Object);

        Assert.IsType<NotFoundResult>(result);

    }

    [Theory]
    [AutoControllerDomainData]
    public void GivenDevelopmentEnvironment_WhenHandleErrorDevelopment_ThenReturnProblem(
        Mock<IHostEnvironment> hostEnvironmentMock,
        ErrorController controller
    )
    {
        hostEnvironmentMock.Setup(x => x.EnvironmentName).Returns("Development");

        var result = controller.HandleErrorDevelopment(hostEnvironmentMock.Object);

        Assert.IsType<ObjectResult>(result);
    }
}