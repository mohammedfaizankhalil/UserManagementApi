using UserManagementApi.Models;
using UserManagementApi.DTOs;
using UserManagementApi.Repositories;

namespace UserManagementApi.Services;

public class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<User>> RegisterAsync(RegisterUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || request.Username.Length < 3)
        {
            return Result<User>.Failure("Username must be at least 3 characters.");
        }

        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains('@') || !request.Email.Contains('.'))
        {
            return Result<User>.Failure("Invalid email address.");
        }

        if (request.Age < 18)
        {
            return Result<User>.Failure("User must be 18 or older.");
        }

        if (await _repository.EmailExistsAsync(request.Email!))
        {
            return Result<User>.Failure("A user with this email already exists.");
        }

        var newUser = new User(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            request.Username!,
            request.Email!,
            request.Age
        );

        await _repository.AddAsync(newUser);

        return Result<User>.Success(newUser);
    }

    public async Task<Result<User>> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user is null)
        {
            return Result<User>.Failure($"User with ID '{id}' was not found.");
        }

        return Result<User>.Success(user);
    }

    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        bool isDeleted = await _repository.DeleteAsync(id);

        if (!isDeleted)
        {
            return Result<bool>.Failure($"User with ID '{id}' was not found.");
        }

        return Result<bool>.Success(true);
    }

}