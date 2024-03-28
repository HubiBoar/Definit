using OneOf.Else;
using OneOf;

namespace Definit.NewNewResults;

public sealed class Result<T0, T1> : IResult
    where T0 : notnull
    where T1 : notnull
{
    public static Type[] Types => [ typeof(T0), typeof(T1), typeof(Error) ];

    public int Index { get; }
    public object Value  { get; }

    private readonly T0? _value0;
    private readonly T1? _value1;
    private readonly Error? _error;

    private Result(int index, object value) { Index = index; Value = value; }
    private Result(T0 value) : this(0, value) { _value0 = value; }
    private Result(T1 value) : this(1, value) { _value1 = value; }
    public Result(Error error) : this(2, error) { _error = error; }
    
    //public static implicit operator Result<T0, T1>(T0 value)        => new (value);
    //public static implicit operator Result<T0, T1>(T1 value)        => new (value);
    public static implicit operator Result<T0, T1>(Error value)     => new (value);
    public static implicit operator Result<T0, T1>(Exception value) => new (value);
    public static implicit operator Result(Result<T0, T1> value)    => value.Match(_ => Result.Success, _ => Result.Success, e => e);

    public static implicit operator Result<T0, T1>(Result<T0> value)           => value.Match<Result<T0, T1>>(x => new (x), y => new (y));
    public static implicit operator Result<T0, T1>(Result<T1> value)           => value.Match<Result<T0, T1>>(x => new (x), y => new (y));
    public static implicit operator Result<T0, T1>(Result<T1, T0> value)       => value.Match<Result<T0, T1>>(x => new (x), y => new (y), e => e);

    public static implicit operator Task<Result>(Result<T0, T1> value)         => Task.FromResult((Result)value);
    public static implicit operator Task<Result<T0, T1>>(Result<T0, T1> value) => Task.FromResult(value);


    public static Result<T0, T1> Try(Func<Builder> func)
    {
        try
        {
            return func().Value.Match<Result<T0, T1>>(x => new (x), y => new (y), e => e);
        }
        catch (Exception exception)
        {
            return exception;
        }
    }

    public static Task<Result<T0, T1>> Try(Func<Task<Builder>> func) => Try(async () => await func());

    private bool TryGetValue(out OneOf<T0, T1> value, out Error error)
    {
        value = default!;
        error = null!;
        if(Index == 0)
        {
            value = _value0!;
            return true;
        }
        if(Index == 1)
        {
            value = _value1!;
            return true;
        }

        error = _error!;
        return false;
    }

    public OneOfElse<OneOf<T0, T1>> Is(out Error error) => new (TryGetValue(out var value, out error), value);
    public OneOfElse<Error> Is(out OneOf<T0, T1> value) => new (TryGetValue(out value, out var error), error);

    public T Match<T>(Func<T0, T> onValue0, Func<T1, T> onValue1, Func<Error, T> onError)
    {
        try
        {
            return Index switch
            {
                0 => onValue0(_value0!),
                1 => onValue1(_value1!),
                _ => onError(_error!)
            };
        }
        catch (Exception exception)
        {
            return onError(exception);
        }
    }

    // public Result Match(Func<TValue, Result> onValue)                  => Match(onValue, error => error);
    // public Task<Result> Match(Func<TValue, Task<Result>> onValue)      => Match(onValue, error => new Result(error));

    // public Result<T> Match<T>(Func<TValue, Result<T>> onValue) where T : notnull             => Match(onValue, error => error);
    // public Task<Result<T>> Match<T>(Func<TValue, Task<Result<T>>> onValue) where T : notnull => Match(onValue, error => new Result<T>(error));

    public sealed class Builder
    {
        internal OneOf<T0, T1, Error> Value { get; }

        public Builder(T0 value)    { Value = value; }
        public Builder(T1 value)    { Value = value; }
        public Builder(Error value) { Value = value; }

        public static implicit operator Builder(T0 value)            => new (value);
        public static implicit operator Builder(T1 value)            => new (value);
        public static implicit operator Builder(Error value)         => new (value);
        public static implicit operator Task<Builder>(Builder value) => Task.FromResult(value);
    }

}