namespace Definit.NewResults;

public sealed class Success
{
    public static Success Instance { get; } = new Success();

    private Success()
    {
    }
}

public static class ResultExtensions
{
    public static async Task Force(this Task<Result> result, TryContext context) => (await result).Force(context);
    public static async Task Force(this Task<Result<Success>> result, TryContext context) => (await result).Force(context);

    public static Task<Result<T>> Match<T>(this Task<Result> result, Func<Task<Result<T>>> onValue) where T : notnull => result.Match(onValue);
    public static Task<Result<T>> Match<T>(this Task<Result> result, Func<Result<T>> onValue) where T : notnull => result.Match(onValue);

}

public sealed class Result : ResultBase<Success>
{
    public static Result Success { get; } = new Result(NewResults.Success.Instance);
    public static Task<Result> SuccessTask { get; } = new Result(NewResults.Success.Instance);

    public Result(Success value) : base(value) {}
    public Result(Error error)  : base(error)  {}

    public static implicit operator Result(Success value)         => new (value);
    public static implicit operator Result(Error value)           => new (value);
    public static implicit operator Result(Exception value)       => new (value);
    public static implicit operator Task<Result>(Result value)    => Task.FromResult(value);
    public static implicit operator Result(Result<Success> value) => value.Match(v => v);

    public bool TryGetValue(out Success value) => TryGetValueProtected(out value);
    public bool TryGetValue(out Error value)   => TryGetValueProtected(out value);

    public OneOfElse<Error> IsSuccess() => new (TryGetValueProtected(out Success _), TryGetValue);

    public OneOfElse<Success> Is(out Error error)      => new (TryGetValue(out error), TryGetValueProtected);
    public OneOfElse<Success> IsError(out Error error) => new (TryGetValue(out error), TryGetValueProtected);

    public void Force(TryContext context) => ForceValueProtected(context);

    public T Match<T>(Func<T> onValue, Func<Error, T> onError) => MatchProtected(v => onValue(), onError);

    public Result<T> Match<T>(Func<Result<T>> onValue)             where T : notnull => Match(onValue, error => error);
    public Task<Result<T>> Match<T>(Func<Task<Result<T>>> onValue) where T : notnull => Match(onValue, error => error.ToTaskResult<T>());


}