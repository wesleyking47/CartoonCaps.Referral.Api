using CartoonCaps.Referral.Api.Models;

namespace CartoonCaps.Referral.Api.Services;

public class ReferralService : IReferralService
{
    public string CreateCode(string userId)
    {
        throw new NotImplementedException();
    }

    public string? GetCode(string userId)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ReferralDetails>? GetDetails(string userId)
    {
        throw new NotImplementedException();
    }

    public bool ValidateCode(string code)
    {
        throw new NotImplementedException();
    }
}
