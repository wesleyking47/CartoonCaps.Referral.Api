using CartoonCaps.Referral.Domain.Entities;

namespace CartoonCaps.Referral.Application.Dtos;

public class UserDto
{
    public UserDto() { }

    public UserDto(User user)
    {
        Id = user.Id;
        Name = user.Name;
    }

    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;

}