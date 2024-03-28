using OneOf.Else;
using OneOf;

namespace Definit.NewNewResults;

public interface IResult
{
    public static abstract Type[] Types { get; }

    public int Index { get; }
    public object Value  { get; }
}

public abstract class Result_2_Base<TValue> : IResult
    where TValue : notnull
{
    public static Type[] Types => [ typeof(TValue), typeof(Error) ];

    public int Index { get; }
    public object Value  { get; }

    protected readonly TValue? _value;
    protected readonly Error? _error;

    protected Result_2_Base(int index, object value) { Index = index; Value = value; }
    protected Result_2_Base(TValue value) : this(0, value) { _value = value; }
    protected Result_2_Base(Error error)  : this(1, error) { _error = error; }

    protected bool TryGetValueProtected(out TValue value) { value = _value!; return Index == 0; }
    protected bool TryGetValueProtected(out Error error)  { error = _error!; return Index == 1; }

    protected OneOfElse<Error> IsProtected(out TValue value) => new (TryGetValueProtected(out value), _error!);
    protected OneOfElse<TValue> IsProtected(out Error value) => new (TryGetValueProtected(out value), _value!);

    protected T MatchProtected<T>(Func<TValue, T> onValue, Func<Error, T> onError)
    {
        try
        {
            return Index switch
            {
                0 => onValue(_value!),
                _ => onError(_error!)
            };
        }
        catch (Exception exception)
        {
            return onError(exception);
        }
    }

}
