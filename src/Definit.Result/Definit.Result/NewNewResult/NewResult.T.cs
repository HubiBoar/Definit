using OneOf.Else;
using OneOf;

namespace Definit.NewNewResults;

public sealed class Result<T0> : Result_2_Base<T0>
    where T0 : notnull
{
    private Result(T0 value) : base(value) {}
    public Result(Error error)  : base(error) {}

    //public static implicit operator Result<T0>(T0 value)        => new (value);
    public static implicit operator Result<T0>(Error value)            => new (value);
    public static implicit operator Result<T0>(Exception value)        => new (value);
    public static implicit operator Result(Result<T0> value)           => value.Match(_ => Result.Success);
    public static implicit operator OneOf<T0, Error>(Result<T0> value) => value.Match<OneOf<T0, Error>>(v => v, e => e);
    public static implicit operator OneOf<Error, T0>(Result<T0> value) => value.Match<OneOf<Error, T0>>(v => v, e => e);

    public static implicit operator Task<Result<T0>>(Result<T0> value) => Task.FromResult(value);
    public static implicit operator Task<Result>(Result<T0> value)     => Task.FromResult((Result)value);

    public OneOfElse<Error> Is(out T0 value) => IsProtected(out value);
    public OneOfElse<T0> Is(out Error value) => IsProtected(out value);

    public T Match<T>(Func<T0, T> onValue, Func<Error, T> onError) => MatchProtected(onValue, onError);
    public Result Match(Func<T0, Result> onValue)                  => Match(onValue, error => error);
    public Task<Result> Match(Func<T0, Task<Result>> onValue)      => Match(onValue, error => new Result(error));

    public Result<T> Match<T>(Func<T0, Result<T>> onValue) where T : notnull             => Match(onValue, error => error);
    public Task<Result<T>> Match<T>(Func<T0, Task<Result<T>>> onValue) where T : notnull => Match(onValue, error => new Result<T>(error));


    public static Result<T0> Try(Func<Builder> func)
    {
        try
        {
            return func().Value.Match(x => new Result<T0>(x), e => e);
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    public static Task<Result<T0>> Try(Func<Task<Builder>> func) => Try(async () => await func());

    public sealed class Builder
    {
        internal OneOf<T0, Error> Value { get; }

        public Builder(T0 value)    { Value = value; }
        public Builder(Error value) { Value = value; }

        public static implicit operator Builder(T0 value)            => new (value);
        public static implicit operator Builder(Error value)         => new (value);
        public static implicit operator Task<Builder>(Builder value) => Task.FromResult(value);
    }
}