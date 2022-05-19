namespace Loan2022.Result;

public class PaginatedResult<T> : Result
{
    public PaginatedResult(List<T> data) => this.Data = data;

    public List<T> Data { get; set; }

    internal PaginatedResult(
        bool succeeded,
        List<T> data = null,
        List<string> messages = null,
        long count = 0,
        int page = 1,
        int pageSize = 10)
    {
        this.Data = data;
        this.Page = page;
        this.Succeeded = succeeded;
        this.TotalPages = (int) Math.Ceiling((double) count / (double) pageSize);
        this.TotalCount = count;
    }

    public static PaginatedResult<T> Failure(List<string> messages) => new PaginatedResult<T>(false, messages: messages);

    public static PaginatedResult<T> Success(
        List<T> data,
        long count,
        int page,
        int pageSize)
    {
        return new PaginatedResult<T>(true, data, count: count, page: page, pageSize: pageSize);
    }

    public int Page { get; set; }

    public int TotalPages { get; set; }

    public long TotalCount { get; set; }

    public bool HasPreviousPage => this.Page > 1;

    public bool HasNextPage => this.Page < this.TotalPages;
}