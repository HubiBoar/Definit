using OneOf;

namespace Definit.Results;

public abstract class ResultBase<T0> : Result_Base<T0>
    where T0 : notnull
{
    protected ResultBase(T0 value)    : base(value) {}
    protected ResultBase(Error error) : base(error) {}
    protected ResultBase(OneOf<T0, Error> value) : base(value.Match<OneOf<T0, Error>>(x => x, x => x)) {}

    public T Match<T>(Func<T0, T> result, Func<Error, T> error) => MatchProtected(result, error);
}
