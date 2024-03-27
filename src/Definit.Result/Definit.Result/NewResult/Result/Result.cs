namespace Definit.NewResults;

public sealed class Success
{
    public static Success Instance { get; } = new Success();

    private Success()
    {
    }
}

public partial class Result : Result<Success>
{
    public static Result Success { get; } = new Result(NewResults.Success.Instance);
    public static Task<Result> SuccessTask { get; } = new Result(NewResults.Success.Instance);

    private Result(Success value) : base(value) {}
    public Result(Error error)   : base(error) {}

    public static implicit operator Result(Success value)      => new (value);
    public static implicit operator Result(Error value)        => new (value);
    public static implicit operator Result(Exception value)    => new (value);
    public static implicit operator Task<Result>(Result value) => Task.FromResult(value);
}