
namespace Definit.NewResults;

public partial class Result<TValue>
{
    public partial class Error<TError> : OneOf<TValue, TError, Error>, IOneOf<TValue, Error>, IOneOfT0<TValue, Error>
        where TError : IError
    {
        public Error(TValue value) : base(value) {}
        public Error(TError value) : base(value) {}
        public Error(Error value) : base(value) {}

        public T Match<T>(Func<TValue, T> onT0, Func<Error, T> onT1) => Match(onT0, t1 => onT1(t1.ToError()), onT1);

        public void Switch(Action<TValue> onT0, Action<Error> onT1) => Switch(onT0, t1 => onT1(t1.ToError()), onT1);
    }

}