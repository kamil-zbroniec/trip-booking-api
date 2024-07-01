namespace Shared.Results;

using System.Diagnostics.CodeAnalysis;

public class Result<T> : Result
{
    [MemberNotNullWhen(returnValue: true, member: nameof(Value))]
    public new bool IsSuccess { get; }
    
    public T? Value { get; }

    private Result(bool isSuccess, Error error, T? value = default) : base(isSuccess, error)
    {
        IsSuccess = isSuccess;
        Value = value;
    }
    
    public new static Result<T> Failed(Error error) => new(false, error);

    private static Result<T> Success(T value) => new(true, Error.None, value);
    
    public static implicit operator Result<T>(T value) => Success(value);
}