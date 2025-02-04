using Asp.Versioning;
using CartoonCaps.Referral.Api.Models;
using CartoonCaps.Referral.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartoonCaps.Referral.Api.Controllers.v1;

[ApiVersion(1.0)]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class ReferralsController(IReferralService referralsService) : ControllerBase
{
    private readonly IReferralService _referralsService = referralsService;

    [HttpPost]
    [Route("{userId}/code")]
    [ProducesResponseType<string>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCode([FromRoute] string userId)
    {
        try
        {
            var code = await _referralsService.CreateCodeAsync(userId);

            return CreatedAtAction(nameof(GetCode), new { userId }, code);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the referral code.");
        }
    }

    [HttpGet]
    [Route("{userId}/code")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCode([FromRoute] string userId)
    {
        try
        {
            var code = await _referralsService.GetCodeAsync(userId);
            if (code == null)
            {
                return NotFound("Referral code not found.");
            }

            return Ok(code);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the referral code.");
        }
    }

    [HttpGet]
    [Route("{userId}/Details")]
    [ProducesResponseType<IEnumerable<ReferralRecord>>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetReferralRecords([FromRoute] string userId)
    {
        try
        {
            var details = await _referralsService.GetReferralRecordsAsync(userId);
            if (details == null)
            {
                return NotFound("Referral details not found.");
            }

            return Ok(details);
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while getting the referral details.");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateReferralRecord([FromBody] CreateReferralRecordRequest createReferralRecordRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            await _referralsService.CreateReferralRecordAsync(createReferralRecordRequest);

            return Created();
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while creating the referral record.");
        }
    }
}