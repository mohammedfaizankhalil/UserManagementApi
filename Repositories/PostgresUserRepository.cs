using System.Reflection.Metadata.Ecma335;
using Npgsql;
using UserManagementApi.Models;

namespace UserManagementApi.Repositories;

public class PostgresUserRepository : IUserRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public PostgresUserRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        string sql = "SELECT COUNT(1)FROM app_users WHERE LOWER(email) = LOWER(@email);";

        await using var cmd = _dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@email", email);

        var count = (long)(await cmd.ExecuteScalarAsync()?? 0L);
        return count > 0;
    }

    public async Task<bool> EmailExistsForOtherUserAsync(string email, Guid exludeUserId)
    {
        string sql = """
        SELECT COUNT(1)
        FROM app_users
        WHERE LOWER(email) = LOWER(@email) AND id <> @id;
        """;
        await using var cmd = _dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@email", email);
        cmd.Parameters.AddWithValue("@id", exludeUserId);

        var count = (long)(await cmd.ExecuteScalarAsync() ?? 0L);
        return count > 0;
    }

    public async Task<bool> UpdateAsync(User user)
    {
        string sql = """
        UPDATE app_users
        SET username = @username,
            email = @email,
            age = @age
        WHERE id = @id;
        """;

        await using var cmd = _dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@id",user.Id);
        cmd.Parameters.AddWithValue("@username", user.Username);
        cmd.Parameters.AddWithValue("@email",user.Email);
        cmd.Parameters.AddWithValue("@age",user.Age);

        try{
        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        return rowsAffected > 0;
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            return false;
        }
    }    

    public async Task<bool> AddAsync(User user)
    {
        string sql= """
        INSERT INTO app_users (id, username, email, age, created_at)
        VALUES (@id, @username, @email, @age, @created_at);
        """;

        await using var cmd = _dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@id", user.Id);
        cmd.Parameters.AddWithValue("@username", user.Username);
        cmd.Parameters.AddWithValue("@email", user.Email);
        cmd.Parameters.AddWithValue("@age", user.Age);
        cmd.Parameters.AddWithValue("@created_at", user.CreatedAt);
        
        try{
            await cmd.ExecuteNonQueryAsync();
            return true;
        }
        catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UniqueViolation)
        {
            return false;
        }
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        string sql = """
        SELECT id, username, email, age, created_at
        FROM app_users
        WHERE id = @id;
        """;

        await using var cmd = _dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@id", id);

        await using var reader = await cmd.ExecuteReaderAsync();

        if(!await reader.ReadAsync())
        {
            return null;
        }

        return new User(
            reader.GetGuid(0),
            reader.GetFieldValue<DateTimeOffset>(4),
            reader.GetString(1),
            reader.GetString(2),
            reader.GetInt32(3)
        );
    }

    public async Task<IReadOnlyList<User>> GetAllAsync()
    {
        var _users = new List<User>();
         
         string sql = """
         SELECT id, username, email, age, created_at
         FROM app_users
         ORDER BY created_at DESC;
         """;

         await using var cmd = _dataSource.CreateCommand(sql);
         await using var reader = await cmd.ExecuteReaderAsync();

         while(await reader.ReadAsync())
        {
            var user = new User(
                reader.GetGuid(0),
                reader.GetFieldValue<DateTimeOffset>(4),
                reader.GetString(1),
                reader.GetString(2),
                reader.GetInt32(3)
            );

            _users.Add(user);
        }

        return _users;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        string sql = "DELETE FROM app_users WHERE id = @id;";

        await using var cmd = _dataSource.CreateCommand(sql);
        cmd.Parameters.AddWithValue("@id", id);

        int rowsAffected = await cmd.ExecuteNonQueryAsync();
        //Returns true if a row was found and removed, false otherwise
        return rowsAffected > 0;
    }
}