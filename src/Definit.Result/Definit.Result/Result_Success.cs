using OneOf;

namespace Definit.Results;

public sealed partial class Result : ResultBase<Success>
{
    public static Result Success { get; } = new Result(Results.Success.Instance);
    private Result(Success value) : base(value) {}
    public Result(Error error)   : base(error) {}


    public static implicit operator Result(Success _)       => Success;
    public static implicit operator Result(Error value)     => new (value);
    public static implicit operator Result(Exception value) => new (value);

    public static implicit operator Result(Result<Success> value)       => value.Match(v => Success, e => e);
    public static implicit operator Result<Success>(Result value)       => value.Match(v => Success, e => e);
    public static implicit operator Result(OneOf<Success, Error> value) => value.Match<Result>(x => x, x => x);

    public static implicit operator Task<Result>(Result value) => Task.FromResult(value);
}