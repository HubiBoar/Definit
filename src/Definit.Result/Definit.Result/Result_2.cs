using OneOf.Else;
using OneOf;

namespace Definit.Results;

public sealed class Result<T0, T1> : ResultBase<T0, T1>
    where T0 : notnull
    where T1 : notnull
{
    public Result(T0 value)    : base(value) {}
    public Result(T1 value)    : base(value) {}
    public Result(Error error) : base(error) {}

    public static implicit operator Result<T0, T1>(T0 value)        => new (value);
    public static implicit operator Result<T0, T1>(T1 value)        => new (value);
    public static implicit operator Result<T0, T1>(Error value)     => new (value);
    public static implicit operator Result<T0, T1>(Exception value) => new (value);

    public static implicit operator Result(Result<T0, T1> value)         => value.Match(v => Result.Success, v => Result.Success, e => e);
    public static implicit operator Result<T0, T1>(Result<T0> value)     => value.Match<Result<T0, T1>>(x => x, x => x);
    public static implicit operator Result<T0, T1>(Result<T1> value)     => value.Match<Result<T0, T1>>(x => x, x => x);

    public static implicit operator Result<T1, T0>(Result<T0, T1> value) => value.Match<Result<T1, T0>>(x => x, x => x, x => x);

    public static implicit operator Result<T0, T1>(OneOf<T1, T0> value)  => value.Match<Result<T0, T1>>(x => x, x => x);
    public static implicit operator Result<T0, T1>(OneOf<T0, T1> value)  => value.Match<Result<T0, T1>>(x => x, x => x);

    public static implicit operator Result<T0, T1>(OneOf<T0, Error> value)     => value.Match<Result<T0, T1>>(x => x, x => x);
    public static implicit operator Result<T0, T1>(OneOf<T1, Error> value)     => value.Match<Result<T0, T1>>(x => x, x => x);
    public static implicit operator Result<T0, T1>(OneOf<T0, T1, Error> value) => value.Match<Result<T0, T1>>(x => x, x => x, x => x);
    public static implicit operator Result<T0, T1>(OneOf<T1, T0, Error> value) => value.Match<Result<T0, T1>>(x => x, x => x, x => x);

    public static implicit operator Task<Result<T0, T1>>(Result<T0, T1> value) => Task.FromResult(value);
}