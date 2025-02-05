using Asp.Versioning;
using CartoonCaps.Referral.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartoonCaps.Referral.Api.Controllers.v1;

[ApiVersion("1.0")]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/v{version:apiVersion}/[controller]")]
public class CodesController(IReferralService referralsService) : ControllerBase
{
    private readonly IReferralService _referralsService = referralsService;

    [HttpPost]
    [Route("{userId}")]
    public async Task<ActionResult<string>> PostAsync([FromRoute] string userId)
    {
        var code = await _referralsService.CreateCodeAsync(userId);
        if (code == null)
        {
            return BadRequest("Code not created.");
        }

        return CreatedAtAction(nameof(GetAsync), new { userId }, code);
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<ActionResult<string>> GetAsync([FromRoute] string userId)
    {
        var code = await _referralsService.GetCodeAsync(userId);
        if (code == null)
        {
            return NotFound("Referral code not found.");
        }

        return code;
    }
}