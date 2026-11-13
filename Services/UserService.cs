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

    public async Task<Result<UserResponse>> RegisterAsync(RegisterUserRequest request)
    {
        var validationError = ValidateUserFields(request.Username, request.Email, request.Age);

        if (validationError is not null)
        {
            return Result<UserResponse>.Failure(validationError, ErrorType.Validation);
        }

        if(await _repository.EmailExistsAsync(request.Email!))
        {
            return Result<UserResponse>.Failure("A user with this email already exists!", ErrorType.Conflict);
        }

        var newUser = new User(
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            request.Username!,
            request.Email!,
            request.Age
        );

        await _repository.AddAsync(newUser);

        return Result<UserResponse>.Success(UserResponse.FromUser(newUser));
    }

    public async Task<Result<UserResponse>> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);

        if (user is null)
        {
            return Result<UserResponse>.Failure($"User with ID '{id}' was not found.", ErrorType.NotFound);
        }

        return Result<UserResponse>.Success(UserResponse.FromUser(user));
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync();
        return users.Select(UserResponse.FromUser).ToList();
    }

    public async Task<Result<UserResponse>> UpdateAsync(Guid id, UpdateUserRequest request)
    {
        var validationError = ValidateUserFields(request.Username, request.Email, request.Age);
        if(validationError is not null)
        {
            return Result<UserResponse>.Failure(validationError,ErrorType.Validation);
        }

        var existingUser = await _repository.GetByIdAsync(id);
        if(existingUser is null)
        {
            return Result<UserResponse>.Failure($"User with ID '{id} was not found.",ErrorType.NotFound);
        }

        if(await _repository.EmailExistsForOtherUserAsync(request.Email!, id))
        {
            return Result<UserResponse>.Failure("Another user with this email already exists!", ErrorType.Conflict);
        }

        existingUser.Update(request.Username!, request.Email!, request.Age);
        await _repository.UpdateAsync(existingUser);

        return Result<UserResponse>.Success(UserResponse.FromUser(existingUser));
    }
    public async Task<Result<bool>> DeleteAsync(Guid id)
    {
        bool isDeleted = await _repository.DeleteAsync(id);

        if (!isDeleted)
        {
            return Result<bool>.Failure($"User with ID '{id}' was not found.",ErrorType.NotFound);
        }

        return Result<bool>.Success(true);
    }

    public static string? ValidateUserFields(string? username, string? email, int age)
    {
        if(string.IsNullOrWhiteSpace(username) || username.Length is < 3)
        {
            return "Username must be at least 3 characters.";
        }

        if(string.IsNullOrWhiteSpace(email) || !email.Contains('@') || !email.Contains('.'))
        {
            return "Invalid email address!";
        }

        if(age < 18)
        {
            return "User must be 18 or above!";
        }

        return null;
    }

}