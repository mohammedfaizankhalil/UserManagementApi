namespace UserManagementApi.Models;

public class User
{
    public Guid Id { get; }
    public DateTimeOffset CreatedAt { get; }
    public string Username { get; }
    public string Email { get; }
    public int Age { get; }

    public User(Guid id, DateTimeOffset createdAt, string username, string email, int age)
    {
        Id = id;
        CreatedAt = createdAt;
        Username = username;
        Email = email;
        Age = age;
    }

}