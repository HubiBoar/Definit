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


public sealed class Result : Result_2_Base<Success>
{
    public static Result Success { get; } = new Result(NewNewResults.Success.Instance);
    public static Task<Result> SuccessTask { get; } = new Result(NewNewResults.Success.Instance);

    private Result(Success value) : base(value) {}
    public Result(Error error)  : base(error)  {}

    //public static implicit operator Result(Success value)         => new (value);
    public static implicit operator Result(Error value)           => new (value);
    public static implicit operator Result(Exception value)       => new (value);
    public static implicit operator Task<Result>(Result value)    => Task.FromResult(value);
    public static implicit operator Result(Result<Success> value) => value.Match(v => v);

    public OneOfElse<Error> IsSuccess() => new (TryGetValueProtected(out Success _), _error!);

    public OneOfElse<Success> Is(out Error error)      => new (TryGetValueProtected(out error), _value!);
    public OneOfElse<Success> IsError(out Error error) => new (TryGetValueProtected(out error), _value!);

    public T Match<T>(Func<T> onValue, Func<Error, T> onError) => MatchProtected(v => onValue(), onError);
    public Result<T> Match<T>(Func<Result<T>> onValue)             where T : notnull => Match(onValue, error => error);
    public Task<Result<T>> Match<T>(Func<Task<Result<T>>> onValue) where T : notnull => Match(onValue, error => new Result<T>(error));

    
    public static Result Try(Func<Builder> func)
    {
        try
        {
            return func().Value.Match(x => new Result(x), e => e);
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    public static Task<Result> Try(Func<Task<Builder>> func) => Try(async () => await func());

    public sealed class Builder
    {
        internal OneOf<Success, Error> Value { get; }

        public Builder(Success value) { Value = value; }
        public Builder(Error value)   { Value = value; }

        public static implicit operator Builder(Success value)       => new (value);
        public static implicit operator Builder(Error value)         => new (value);
        public static implicit operator Task<Builder>(Builder value) => Task.FromResult(value);
    }

}