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
    public async Task<ActionResult> PostAsync([FromBody] ReferralRecordRequest referralRecordRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _referralsService.CreateReferralRecordAsync(referralRecordRequest);

        return Created();
    }

    [HttpGet]
    [Route("{userId}")]
    public async Task<ActionResult<ReferralRecordResponse>> GetAsync([FromRoute] int userId)
    {
        var response = await _referralsService.GetReferralRecordsAsync(userId);
        if (response.ReferralRecords == null)
        {
            return NotFound("Referral details not found.");
        }

        return response;
    }
}