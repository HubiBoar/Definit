using OneOf.Else;
using OneOf;

namespace Definit.NewNewResults;

public sealed class Success
{
    public static Success Instance { get; } = new Success();

    private Success()
    {
    }
}



public static class Result_Success_Extensions
{
    public static async Task Force(this Task<Result> result, TryContext context) => (await result).Force(context);
}

public sealed class Result : Result_Base<Success>
{
    public static Builder Success { get; } = new Builder(new Result(NewNewResults.Success.Instance));

    private Result(Success value) : base(value) {}
    private Result(Error error)  : base(error)  {}

    public static implicit operator Task<Result>(Result value)    => Task.FromResult(value);
    public static implicit operator Result(Result<Success> value) => Try(_ => value.Match<Builder>(v => v, e => e));

    public OneOfElse<Error> IsSuccess() => new (TryGetValueProtected(out Success _), _error!);

    public OneOfElse<Success> Is(out Error error)      => new (TryGetValueProtected(out error), _value!);
    public OneOfElse<Success> IsError(out Error error) => new (TryGetValueProtected(out error), _value!);

    public T Match<T>(Func<T> onValue, Func<Error, T> onError) => MatchProtected(v => onValue(), onError);
    public void Match(Action onValue, Action<Error> onError) => SwitchProtected(v => onValue(), onError);

    public void Force(TryContext context) => Match(() => {}, error => throw error.ToException(context));

    public static Result Try(Func<TryContext, Builder> func)
    {
        try
        {
            return func(TryContext.Instance).Value;
        }
        catch (Exception exception)
        {
            return new Result(exception);
        }
    }

    public static Task<Result> Try(Func<TryContext, Task<Builder>> func) => Try(async (context) => await func(context));

    public sealed class Builder
    {
        internal Result Value { get; }

        public Builder(Success value) { Value = new Result(value); }
        public Builder(Error value)   { Value = new Result(value); }
        public Builder(Result value)   { Value = value; }

        public static implicit operator Builder(Success value)       => new (value);
        public static implicit operator Builder(Error value)         => new (value);
        public static implicit operator Builder(Exception value)     => new (value);
        public static implicit operator Builder(Result value)        => new (value);
        public static implicit operator Task<Builder>(Builder value) => Task.FromResult(value);
    }

}