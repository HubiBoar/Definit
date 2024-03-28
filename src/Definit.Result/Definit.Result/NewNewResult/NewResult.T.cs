using OneOf.Else;

namespace Definit.NewNewResults;

public static class Result_T_Extensions
{
    public static async Task<T> Force<T>(this Task<Result<T>> result, TryContext context) where T : notnull => (await result).Force(context);
}

public sealed class Result<T0> : Result_Base<T0>
    where T0 : notnull
{
    private Result(T0 value) : base(value) {}
    private Result(Error error)  : base(error) {}

    public static implicit operator Result(Result<T0> value)           => Result.Try(c => value.Match(_ => Result.Success, e => e));
    public static implicit operator Result.Builder(Result<T0> value)   => (Result)value;

    public static implicit operator Task<Result<T0>>(Result<T0> value) => Task.FromResult(value);
    public static implicit operator Task<Result>(Result<T0> value)     => Task.FromResult((Result)value);
    public static implicit operator Task<Result.Builder>(Result<T0> value) => Task.FromResult((Result.Builder)value);

    public OneOfElse<Error> Is(out T0 value) => IsProtected(out value);
    public OneOfElse<T0> Is(out Error value) => IsProtected(out value);

    public T Match<T>(Func<T0, T> onValue, Func<Error, T> onError) => MatchProtected(onValue, onError);
    public void Match(Action<T0> onValue, Action<Error> onError) => SwitchProtected(onValue, onError);

    public T0 Force(TryContext context) => Match(v => v, error => throw error.ToException(context));

    public static Result<T0> Try(Func<TryContext, Builder> func)
    {
        try
        {
            return func(TryContext.Instance).Value;
        }
        catch (Exception exception)
        {
            return new Result<T0>(exception);
        }
    }

    public static Task<Result<T0>> Try(Func<TryContext, Task<Builder>> func) => Try(async (context) => await func(context));

    public sealed class Builder
    {
        internal Result<T0> Value { get; }

        public Builder(T0 value)    { Value = new Result<T0>(value); }
        public Builder(Error value) { Value = new Result<T0>(value); }
        public Builder(Result<T0> value) { Value = value; }

        public static implicit operator Builder(T0 value)            => new (value);
        public static implicit operator Builder(Result<T0> value)    => new (value);
        public static implicit operator Builder(Error value)         => new (value);
        public static implicit operator Builder(Exception value)     => new (value);
        public static implicit operator Task<Builder>(Builder value) => Task.FromResult(value);
    }
}