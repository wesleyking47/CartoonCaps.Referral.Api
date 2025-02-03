using Asp.Versioning;
using CartoonCaps.Referral.Api.Services;
using CartoonCaps.Referral.Api.Models;
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
    [Route("code")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateCode([FromBody] CreateCodeRequest request)
    {
        try
        {
            var userId = request.UserId;

            var userExists = _userService.Exists(userId);
            if (!userExists)
            {
                return BadRequest("User does not exist.");
            }

            var code = _referralsService.CreateCode(userId);

            return CreatedAtAction(nameof(GetCode), new { userId }, code);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while creating the referral code.");
        }
    }

    [HttpGet]
    [Route("{userId}/code")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetCode([FromRoute] string userId)
    {
        try
        {
            var code = _referralsService.GetCode(userId);
            if (code == null)
            {
                return NotFound("Referral code not found.");
            }

            return Ok(code);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while getting the referral code.");
        }
    }

    [HttpGet]
    [Route("{userId}/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetReferralStatus([FromRoute] string userId)
    {
        return Ok();
    }
}