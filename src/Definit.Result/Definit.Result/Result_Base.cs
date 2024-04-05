using OneOf;
using OneOf.Else;

namespace Definit.Results;

public abstract class Result_Base<TResult>
    where TResult : notnull
{
    public OneOf<TResult, Error> Value { get; }

    protected Result_Base(OneOf<TResult, Error> result)
    {
        Value = result;
    }

    public OneOfElse<TResult> Is(out Error error) => Value.Is(out error);
    public OneOfElse<Error> Is(out TResult value) => Value.Is(out value);

    protected T MatchProtected<T>(Func<TResult, T> result, Func<Error, T> error) => Value.TryMatch(v => result(v), e => error(e), e => error(e));
}