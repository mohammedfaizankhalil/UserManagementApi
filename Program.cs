using Npgsql;
using UserManagementApi.DTOs;
using UserManagementApi.Models;
using UserManagementApi.Repositories;
using UserManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Database Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 3. Register Services & Dependencies
// Singleton: One connection pool shared globally across all HTTP requests
builder.Services.AddSingleton(sp => NpgsqlDataSource.Create(connectionString));

// Scoped: Created once per incoming HTTP request
builder.Services.AddScoped<IUserRepository, PostgresUserRepository>();//This tells ASP.NET Core: "Whenever any class asks for an IUserRepository, instantiate a PostgresUserRepository."
builder.Services.AddScoped<UserService>();
//When an HTTP request arrives at your API endpoint requiring UserService:The DI container inspects UserService's constructor and sees: UserService(IUserRepository repository).It checks its service registry: "Who implements IUserRepository?" $\rightarrow$ Answer: PostgresUserRepository.

var app = builder.Build();

// 4. HTTP Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 5. HTTP Endpoints (APIs)
app.MapPost("/api/users", async (RegisterUserRequest request, UserService userService) =>
{
    var result = await userService.RegisterAsync(request);

    if (!result.IsSuccess)
    {
        return result.ErrorType == ErrorType.Conflict
        ? Results.Conflict(new {error = result.Error})
        : Results.BadRequest(new{error = result.Error});
    }

    return Results.Created($"/api/users/{result.Value!.id}", result.Value);
})
.WithName("RegisterUser");

app.MapPut("/api/users/{id:guid}", async (Guid id, UpdateUserRequest request, UserService userService) =>
{
    var result = await userService.UpdateAsync(id, request);

    if (!result.IsSuccess)
    {
        return result.ErrorType switch
        {
            ErrorType.NotFound => Results.NotFound(new { error = result.Error }),
            ErrorType.Conflict => Results.Conflict(new { error = result.Error }),
            _ => Results.BadRequest(new { error = result.Error })
        };
    }

    return Results.Ok(result.Value);
})
.WithName("UpdateUser");

app.MapGet("/api/users/{id:guid}", async (Guid id, UpdateUserRequest request, UserService userService) =>
{
    var result = await userService.GetByIdAsync(id);

    if (!result.IsSuccess)
    {
        return Results.NotFound(new { error = result.Error });
    }

    return Results.Ok(result.Value);
})
.WithName("GetUserById");

app.MapGet("/api/users/", async (UserService userService) =>
{
    var result = await userService.GetAllAsync();

    return Results.Ok(result);
})
.WithName("GetAllUsers");

app.MapDelete("/api/users/{id:guid}", async (Guid id, UserService userService) =>
{
    var result = await userService.DeleteAsync(id);

    if (!result.IsSuccess)
    {
        return Results.NotFound(new{error= result.Error});
    }

    return Results.NoContent();
})
.WithName("DeleteUser");

app.Run();