using OneOf;

namespace Definit.Results;

public sealed class Result<T0> : ResultBase<T0>
    where T0 : notnull
{
    public Result(T0 value)    : base(value) {}
    public Result(Error error) : base(error) {}

    public static implicit operator Result<T0>(T0 value)        => new (value);
    public static implicit operator Result<T0>(Error value)     => new (value);
    public static implicit operator Result<T0>(Exception value) => new (value);
    public static implicit operator Result(Result<T0> value)    => value.Match(v => Result.Success, e => e);

    public static implicit operator Result<T0>(OneOf<T0, Error> value) => value.Match<Result<T0>>(v => v, e => e);

    public static implicit operator Task<Result<T0>>(Result<T0> value) => Task.FromResult(value);
}