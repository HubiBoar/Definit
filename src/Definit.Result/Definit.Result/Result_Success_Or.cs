using OneOf;

namespace Definit.Results;

public sealed partial class Result
{
    public sealed class Or<T1> : ResultBase<Success, T1>
        where T1 : notnull
    {
        public static Or<T1> Success { get; } = new Or<T1>(Results.Success.Instance);

        private Or(Success value) : base(value) {}
        public Or(T1 value)       : base(value) {}
        public Or(Error error)    : base(error) {}

        public static implicit operator Or<T1>(Success _)       => Success;
        public static implicit operator Or<T1>(T1 value)        => new (value);
        public static implicit operator Or<T1>(Error value)     => new (value);
        public static implicit operator Or<T1>(Exception value) => new (value);

        public static implicit operator Or<T1>(Result value)     => value.Match<Or<T1>>(x => x, x => x);
        public static implicit operator Or<T1>(Result<T1> value) => value.Match<Or<T1>>(x => x, x => x);

        public static implicit operator Or<T1>(OneOf<Success, T1> value)    => value.Match<Or<T1>>(x => x, x => x);
        public static implicit operator Or<T1>(OneOf<T1, Success> value)    => value.Match<Or<T1>>(x => x, x => x);
        public static implicit operator Or<T1>(OneOf<Success, Error> value) => value.Match<Or<T1>>(x => x, x => x);
        public static implicit operator Or<T1>(OneOf<T1, Error> value)      => value.Match<Or<T1>>(x => x, x => x);

        public static implicit operator Or<T1>(Result<Success, T1> value) => value.Match<Or<T1>>(x => x, x => x, x => x);
        public static implicit operator Or<T1>(Result<T1, Success> value) => value.Match<Or<T1>>(x => x, x => x, x => x);

        public static implicit operator Result<Success, T1>(Or<T1> value) => value.Match<Result<Success, T1>>(x => x, x => x, x => x);
        public static implicit operator Result<T1, Success>(Or<T1> value) => value.Match<Result<T1, Success>>(x => x, x => x, x => x);

        public static implicit operator Result(Or<T1> value) => value.Match<Result>(x => Results.Success.Instance, x => Results.Success.Instance, x => x);

        public static implicit operator Task<Or<T1>>(Or<T1> value)  => Task.FromResult(value);
    }
}