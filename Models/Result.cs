namespace UserManagementApi.Models;

public enum ErrorType
{
    Validation,
    NotFound,
    Conflict
}
public class Result<T>
{
    public bool IsSuccess {get;}
    public string? Error{get;}
    public ErrorType ErrorType{get;}
    public T? Value{get;}

    private Result(bool isSuccess, string? error, ErrorType errorType, T? value)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorType = errorType;
        Value = value;
    }

    public static Result<T> Failure(string error, ErrorType errorType) => new (false,error,errorType,default);
    public static Result<T> Success(T value) => new (true,null,ErrorType.Validation,value);
}