namespace Shared.Results;

public class Result
{
    public Error Error { get; }

    public bool IsSuccess { get; }

    public bool IsFailed => !IsSuccess;
    
    protected Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess; 
        Error = error;
    }

    public static Result Failed(Error error) => new Result(false, error);

    public static Result Success() => new Result(true, Error.None);
}