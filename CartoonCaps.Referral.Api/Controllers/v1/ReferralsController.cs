using Asp.Versioning;
using CartoonCaps.Referral.Api.Models;
using CartoonCaps.Referral.Api.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CartoonCaps.Referral.Api.Controllers.v1;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class ReferralsController(IReferralService referralsService, IUserService userService) : ControllerBase
{
    private readonly IReferralService _referralsService = referralsService;
    private readonly IUserService _userService = userService;

    [HttpPost]
    [Route("{userId}/code")]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
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

            var codeExists = _referralsService.GetCode(userId) != null;
            if (codeExists)
            {
                return Conflict("Referral code already exists.");
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
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
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
    [Route("{userId}/Details")]
    [ProducesResponseType<IEnumerable<ReferralDetails>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult GetReferralDetails([FromRoute] string userId)
    {
        try
        {
            var details = _referralsService.GetDetails(userId);
            if (details == null)
            {
                return NotFound("Referral details not found.");
            }

            return Ok(details);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while getting the referral details.");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult CreateReferralRecord([FromBody] CreateReferralRecordRequest createReferralRecordRequest)
    {
        try
        {
            var isValidCode = _referralsService.ValidateCode(createReferralRecordRequest.ReferralCode);
            if (!isValidCode)
            {
                return BadRequest("Invalid referral code.");
            }

            return Created();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occured while creating the referral record.");
        }
    }
}