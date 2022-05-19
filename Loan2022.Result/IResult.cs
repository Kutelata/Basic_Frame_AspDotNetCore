namespace Loan2022.Result;

public interface IResult
{
    string Message { get; set; }

    bool Succeeded { get; set; }
}