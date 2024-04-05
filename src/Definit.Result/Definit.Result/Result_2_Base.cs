using OneOf;
using OneOf.Else;

namespace Definit.Results;

public abstract class ResultBase<T0, T1> : Result_Base<OneOf<T0, T1>>
    where T0 : notnull
    where T1 : notnull
{
    protected ResultBase(T0 value)    : base((OneOf<T0, T1>)value) {}
    protected ResultBase(T1 value)    : base((OneOf<T0, T1>)value) {}
    protected ResultBase(Error error) : base(error) {}
    protected ResultBase(OneOf<T0, T1> value) : base(value) {}
    protected ResultBase(OneOf<T0, T1, Error> value) : base(value.Match<OneOf<OneOf<T0, T1>, Error>>(x => (OneOf<T0, T1>)x, x => (OneOf<T0, T1>)x, x => x)) {}

    public OneOfElse<OneOf<T1, Error>> Is(out T0 value)
    {
        if(Value.TryPickT0(out var oneOf, out var error))
        {
            return new (oneOf.Is(out value).Else(out var remainder), remainder);
        }

        value = default!;
        return new (true, error);
    }

    public OneOfElse<OneOf<T0, Error>> Is(out T1 value)
    {
        if(Value.TryPickT0(out var oneOf, out var error))
        {
            return new (oneOf.Is(out value).Else(out var remainder), remainder);
        }

        value = default!;
        return new (true, error);
    }

    public T Match<T>(Func<T0, T> t0, Func<T1, T> t1, Func<Error, T> error) => MatchProtected(v => v.Match(x => t0(x), x => t1(x)), error);
    public Task<T> Match<T>(Func<T0, Task<T>> t0, Func<T1, Task<T>> t1, Func<Error, T> error) => Match(x => t0(x), x => t1(x), e => Task.FromResult(error(e)));
    public Task<T> Match<T>(Func<T0, Task<T>> t0, Func<T1, T> t1, Func<Error, T> error) => Match(x => t0(x), x => Task.FromResult(t1(x)), e => Task.FromResult(error(e)));
    public Task<T> Match<T>(Func<T0, T> t0, Func<T1, Task<T>> t1, Func<Error, T> error) => Match(x => Task.FromResult(t0(x)), x => t1(x), e => Task.FromResult(error(e)));

    public Task<T> Match<T>(Func<T0, T> t0, Func<T1, Task<T>> t1, Func<Error, Task<T>> error) => Match(x => Task.FromResult(t0(x)), x => t1(x), e => error(e));
    public Task<T> Match<T>(Func<T0, Task<T>> t0, Func<T1, T> t1, Func<Error, Task<T>> error) => Match(x => t0(x), x =>Task.FromResult(t1(x)), e => error(e));
}