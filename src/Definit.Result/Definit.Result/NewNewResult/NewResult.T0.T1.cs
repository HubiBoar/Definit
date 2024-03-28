using OneOf.Else;
using OneOf;

namespace Definit.NewNewResults;

public static class Result_T0_T1_Extensions
{
    public static async Task<OneOf<T0, T1>> Force<T0, T1>(this Task<Result<T0, T1>> result, TryContext context) where T0 : notnull where T1 : notnull => (await result).Force(context);
}

//Maybe change to Result_Base
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
    private Result(Error error) : this(2, error) { _error = error; }
    
    public static implicit operator Result(Result<T0, T1> value)               => Result.Try(c => value.Match(_ => Result.Success, _ => Result.Success, e => e));
    public static implicit operator Result<T0, T1>(Result<T0> value)           => Try(c => value.Match<Builder>(x => x, y => y));
    public static implicit operator Result<T0, T1>(Result<T1> value)           => Try(c => value.Match<Builder>(x => x, y => y));
    public static implicit operator Result<T0, T1>(Result<T1, T0> value)       => Try(c => value.Match<Builder>(x => x, y => y, e => e));

    public static implicit operator Task<Result>(Result<T0, T1> value)         => Task.FromResult((Result)value);
    public static implicit operator Task<Result<T0, T1>>(Result<T0, T1> value) => Task.FromResult(value);

    public OneOfElse<OneOf<T0, T1>> Is(out Error error) => new (TryGetValue(out var value, out error), value);
    public OneOfElse<Error> Is(out OneOf<T0, T1> value) => new (TryGetValue(out value, out var error), error);


    public OneOf<T0, T1> Force(TryContext context) => Match<OneOf<T0, T1>>(v => v, v => v, error => throw error.ToException(context));

    public static Task<Result<T0, T1>> Try(Func<TryContext, Task<Builder>> func) => Try(async (context) => await func(context));
    public static Result<T0, T1> Try(Func<TryContext, Builder> func)
    {
        try
        {
            return func(TryContext.Instance).Value;
        }
        catch (Exception exception)
        {
            return new Result<T0, T1>(exception);
        }
    }

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

    public sealed class Builder
    {
        internal Result<T0, T1> Value { get; }

        public Builder(T0 value)    { Value = new Result<T0, T1>(value); }
        public Builder(T1 value)    { Value = new Result<T0, T1>(value); }
        public Builder(Error value) { Value = new Result<T0, T1>(value); }
        public Builder(Result<T0, T1> value) { Value = value; }

        public static implicit operator Builder(T0 value)             => new (value);
        public static implicit operator Builder(T1 value)             => new (value);
        public static implicit operator Builder(Result<T0> value)     => new (value);
        public static implicit operator Builder(Result<T1> value)     => new (value);
        public static implicit operator Builder(OneOf<T0, T1> value)  => value.Match<Builder>(x => x, x => x);
        public static implicit operator Builder(OneOf<T1, T0> value)  => value.Match<Builder>(x => x, x => x);
        public static implicit operator Builder(Result<T0, T1> value) => new (value);
        public static implicit operator Builder(Result<T1, T0> value) => new (value);
 
        public static implicit operator Builder(Error value)          => new (value);
        public static implicit operator Builder(Exception value)      => new (value);
        public static implicit operator Task<Builder>(Builder value)  => Task.FromResult(value);
    }

}