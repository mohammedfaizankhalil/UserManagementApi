namespace UserManagementApi.DTOs;
public record UpdateUserRequest(string? Username, string? Email, int Age);