namespace CartoonCaps.Referral.Domain.Entities;

public class User(string id, string name)
{
    public string Id { get; set; } = id;
    public string Name { get; set; } = name;
}