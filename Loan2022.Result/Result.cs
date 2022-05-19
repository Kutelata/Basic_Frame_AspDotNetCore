namespace Loan2022.Result;

public class Result : IResult
{
    public bool Failed => !this.Succeeded;

    public string Message { get; set; }

    public bool Succeeded { get; set; }

    public static IResult Fail() => (IResult) new Result()
    {
        Succeeded = false
    };

    public static IResult Fail(string message) => (IResult) new Result()
    {
        Succeeded = false,
        Message = message
    };

    public static Task<IResult> FailAsync() => Task.FromResult<IResult>(Result.Fail());

    public static Task<IResult> FailAsync(string message) => Task.FromResult<IResult>(Result.Fail(message));

    public static IResult Success() => (IResult) new Result()
    {
        Succeeded = true
    };

    public static IResult Success(string message) => (IResult) new Result()
    {
        Succeeded = true,
        Message = message
    };

    public static Task<IResult> SuccessAsync() => Task.FromResult<IResult>(Result.Success());

    public static Task<IResult> SuccessAsync(string message) => Task.FromResult<IResult>(Result.Success(message));
}