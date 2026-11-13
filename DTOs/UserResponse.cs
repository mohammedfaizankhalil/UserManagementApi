using UserManagementApi.Models;

namespace UserManagementApi.DTOs;

public record UserResponse(
    Guid id,
    string Usernname,
    string Eamil,
    int Age,
    DateTimeOffset CreatedAt)
{
    public static UserResponse FromUser(User user) => new UserResponse(user.Id, user.Username, user.Email, user.Age, user.CreatedAt);
}