using Asp.Versioning;
using CartoonCaps.Referral.Application.Dtos;
using CartoonCaps.Referral.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace CartoonCaps.Referral.Api.Controllers.v1;

[ApiVersion(1.0)]
[ApiController]
[ApiConventionType(typeof(DefaultApiConventions))]
[Route("api/v{version:apiVersion}/[controller]")]
public class ReferralsController(IReferralService referralsService) : ControllerBase
{
    private readonly IReferralService _referralsService = referralsService;

    [HttpPost]
    public async Task<ActionResult> PostAsync([FromBody] ReferralRecordDto referralRecord)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _referralsService.CreateReferralRecordAsync(referralRecord);

        return Created();
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<ActionResult<IEnumerable<ReferralRecordDto>>> GetAsync([FromRoute] string userId)
    {
        var details = await _referralsService.GetReferralRecordsAsync(userId);
        if (details == null)
        {
            return NotFound("Referral details not found.");
        }

        return details.ToList();
    }
}