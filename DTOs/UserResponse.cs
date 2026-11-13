using UserManagementApi.Models;

namespace UserManagementApi.DTOs;

public record UserResponse(
    Guid Id,
    string Username,
    string Email,
    int Age,
    DateTimeOffset CreatedAt)
{
    public static UserResponse FromUser(User user) => new(user.Id, user.Username, user.Email, user.Age, user.CreatedAt);
}