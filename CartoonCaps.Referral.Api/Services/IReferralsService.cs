namespace CartoonCaps.Referral.Api.Services;

public interface IReferralsService
{
    string CreateCode(string userId);
    string GetCode(string userId);
}
