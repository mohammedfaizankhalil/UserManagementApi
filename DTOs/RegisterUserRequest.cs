namespace UserManagementApi.DTOs;

public record RegisterUserRequest(string? Username, string? Email, int Age);