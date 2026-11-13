namespace UserManagementApi.Models;

public class User
{
    public Guid Id { get; }
    public DateTimeOffset CreatedAt { get; }
    public string Username { get; private set;}
    public string Email { get; private set;}
    public int Age { get; private set;}

    public User(Guid id, DateTimeOffset createdAt, string username, string email, int age)
    {
        Id = id;
        CreatedAt = createdAt;
        Username = username;
        Email = email;
        Age = age;
    }

    public void Update(string username, string email, int age)
    {
        Username = username;
        Email = email;
        Age = age;
    }

}