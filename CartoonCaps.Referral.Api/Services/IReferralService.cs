using CartoonCaps.Referral.Api.Models;

namespace CartoonCaps.Referral.Api.Services;

public interface IReferralService
{
    string CreateCode(string userId);
    string? GetCode(string userId);
    IEnumerable<ReferralDetails>? GetDetails(string userId);
    bool ValidateCode(string code);
}
