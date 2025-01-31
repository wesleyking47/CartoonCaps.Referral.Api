using Asp.Versioning;
using CartoonCaps.Referral.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartoonCaps.Referral.Api.Controllers.v1;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class ReferralsController(IReferralsService referralsService, IUserService userService) : ControllerBase
{
    private readonly IReferralsService _referralsService = referralsService;
    private readonly IUserService _userService = userService;

    [HttpPost]
    [Route("{userId}/code")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateCode([FromRoute] string userId)
    {
        try
        {
            var userExists = _userService.Exists(userId);
            if (!userExists)
            {
                return BadRequest("User does not exist.");
            }

            var code = _referralsService.CreateCode(userId);
            if (code == null)
            {
                return BadRequest();
            }

            return Created($"/referrals/{userId}/code", userId);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    [HttpGet]
    [Route("{userId}/code")]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetCode([FromRoute] string userId)
    {
        try
        {
            var code = _referralsService.GetCode(userId);
            if (code == null)
            {
                return NotFound();
            }

            return Ok(code);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}