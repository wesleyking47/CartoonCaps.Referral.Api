using CartoonCaps.Referral.Api.Services;

namespace CartoonCaps.Referral.Api;

public class ReferralsService : IReferralsService
{
    public string CreateCode(string userId)
    {
        return "New Code";
    }

    public string GetCode(string userId)
    {
        return "Existing Code";
    }
}
