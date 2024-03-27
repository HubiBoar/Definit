namespace Definit.NewResults;

public interface IError
{
    public Error ToError();
}

public sealed class ErrorException : Exception
{
    public Error Error { get; }

    public ErrorException(Error error, TryContext context) : base($"{nameof(ErrorException)} :: {error.Message}")
    {
        Error = error;
    }
}

public sealed class TryContext
{
    internal static TryContext Instance { get; } = new TryContext();

    private TryContext()
    {
    }
}

public class Error : IError
{
    public string Message { get; }
    public string StackTrace { get; }

    public Error ToError()
    {
        return this;
    }

    public Error(Exception exception)
    {
        if(exception is ErrorException errorException)
        {
            var error = errorException.Error;

            StackTrace = error.StackTrace ?? Environment.StackTrace;
            Message = error.Message;
        }
        else
        {
           StackTrace = exception.StackTrace ?? Environment.StackTrace;
           Message = exception.Message;
        }
    }

    public ErrorException ToException(TryContext context) => new (this, context);

    public static implicit operator Error(Exception exception) => new (exception);
}

public static class Extensions
{
    public static async Task<Result<TValue>> ToResult<TValue>(this Task<TValue> task) where TValue : notnull => await task;

    public static Func<Error, Task<Result<TValue>>> ToResult<TValue>(this Func<Error, Task<Error>> task) where TValue : notnull => async e => await task(e).ToResult<TValue>();

    public static async Task<Result<TValue>> ToResult<TValue>(this Task<Error> task) where TValue : notnull => await task;
    public static async Task<Result> ToResult(this Task<Error> task) => await task;

    public static async Task<Result> ToResultSuccess(this Task task) { await task; return Result.Success; }

    public static Func<Error, Task<Result>> ToResult(this Func<Error, Error> task) => e => task(e).ToTaskResult();
    public static Func<Error, Task<Result>> ToResult(this Func<Error, Task<Error>> task) => e => task(e).ToResult();
    public static Task<Result<TValue>> ToTaskResult<TValue>(this Error error) where TValue : notnull => Task.FromResult(new Result<TValue>(error));
    public static Task<Result<TValue>> ToTaskResult<TValue>(this ErrorException exception) where TValue : notnull => Task.FromResult(new Result<TValue>(exception.Error));
    public static Task<Result<TValue>> ToTaskResult<TValue>(this Exception exception) where TValue : notnull => Task.FromResult(new Result<TValue>(exception));
    public static Task<Result> ToTaskResult(this Error error) => Task.FromResult(new Result(error));

    public static async Task<TValue> ForceValue<TValue>(this Task<Result<TValue>> result, TryContext context) where TValue : notnull => (await result).ForceValue(context);
}

public partial class Result<TValue> : IOneOfT0<TValue, Error>
    where TValue : notnull
{
    public int Index { get; }
    public object Value  { get; }

    private readonly TValue? _value;
    private readonly Error? _error;

    protected Result(int index, object value)    { Index = index; Value = value; }
    public Result(TValue value) : this(0, value) { _value = value; }
    public Result(Error error)  : this(1, error) { _error = error; }

    public static implicit operator Result<TValue>(TValue value)               => new (value);
    public static implicit operator Result<TValue>(Error value)                => new (value);
    public static implicit operator Result<TValue>(Exception value)            => new (value);
    public static implicit operator Result(Result<TValue> value)               => value.MatchBase(_ => Result.Success, e => new Result(e));
    public static implicit operator Task<Result<TValue>>(Result<TValue> value) => Task.FromResult(value);

    public bool TryGetValue(out TValue value) { value = _value!; return Index == 0; }
    public bool TryGetValue(out Error value)  { value = _error!; return Index == 1; }

    public T MatchBase<T>(Func<TValue, T> onValue, Func<Error, T> onError)
    {
        try
        {
            return Index switch
            {
                0 => onValue((TValue)Value),
                _ => onError((Error)Value)
            };
        }
        catch(ErrorException error)
        {
            return onError(error.Error);
        }
        catch(Exception exception)
        {
            return onError(exception);
        }
    }

    public Result<T> MatchBase<T>(Func<TValue, Result<T>> onValue, Func<Error, Result<T>> onError)
    {
        try
        {
            return Index switch
            {
                0 => onValue((TValue)Value),
                _ => onError((Error)Value)
            };
        }
        catch(ErrorException error)
        {
            return onError(error.Error);
        }
        catch(Exception exception)
        {
            return onError(exception);
        }
    }

    public T Match<T>(Func<TValue, T> onValue, Func<Error, T> onError)                               => MatchBase(onValue, onError);
    public Result<T> Match<T>(Func<TValue, Result<T>> onValue) where T : notnull                             => Match(onValue, error => error);
    public Result<T> Match<T>(Func<TValue, Result<T>> onValue, Func<Error, Error> onError) where T : notnull => MatchBase(onValue, error => new Result<T>(onError(error)));

    public Task<Result<T>> Match<T>(Func<TValue, Task<T>> onValue) where T : notnull                                   => Match(onValue, error => error);
    public Task<Result<T>> Match<T>(Func<TValue, Task<T>> onValue, Func<Error, Error> onError) where T : notnull       => Match(onValue, onError.ToTask());
    public Task<Result<T>> Match<T>(Func<TValue, Task<T>> onValue, Func<Error, Task<Error>> onError) where T : notnull => MatchBase((value) => onValue(value).ToResult(), onError.ToResult<T>());
    public Task<Result<T>> Match<T>(Func<TValue, T> onValue, Func<Error, Task<Error>> onError) where T : notnull       => MatchBase((value) => new Result<T>(onValue(value)), onError.ToResult<T>());

    public Result Switch(Action<TValue> onValue, Func<Error, Error> onError)                 => Match(v => { onValue(v); return Result.Success; }, onError);
    public Task<Result> Switch(Func<TValue, Task> onValue, Func<Error, Task<Error>> onError) => MatchBase(v => onValue(v).ToResultSuccess(), onError.ToResult());
    public Task<Result> Switch(Func<TValue, Task> onValue, Func<Error, Error> onError)       => MatchBase(v => onValue(v).ToResultSuccess(), onError.ToResult());
    public Task<Result> Switch(Action<TValue> onValue, Func<Error, Task<Error>> onError)     => MatchBase(v => { onValue(v); return Result.Success; }, onError.ToResult());

    public TValue ForceValue(TryContext context)
    {
        if(Index == 0)
        {
            return _value!;
        }

        throw _error!.ToException(context);
    }

    public static Result<TValue> Try(Func<TryContext, Result<TValue>> func)
    {
        try
        {
            return func(TryContext.Instance);
        }
        catch(ErrorException error)
        {
            return error.Error;
        }
        catch(Exception exception)
        {
            return exception;
        }
    }

    public static Task<Result<TValue>> Try(Func<TryContext, Task<Result<TValue>>> func)
    {
        try
        {
            return func(TryContext.Instance);
        }
        catch(ErrorException error)
        {
            return error.Error.ToTaskResult<TValue>();
        }
        catch(Exception exception)
        {
            return exception.ToTaskResult<TValue>();
        }
    }
}

public static class Example
{
    public record Value1();
    public record Value2(Value1 Value);

    public static Result<Value2> Test(Result<Value1> result) => Result<Value2>.Try(context =>
    {
        var value = result.ForceValue(context);

        return new Value2(value);
    });

    public static Task<Result<Value2>> TestAsync(Result<Value1> result) => Result<Value2>.Try(async context =>
    {
        var value = result.ForceValue(context);

        await AsyncTask();

        return new Value2(value);
    });

    public static Task<Result<Value2>> TestSync(Result<Value1> result) => Result<Value2>.Try(context =>
    {
        var value = result.ForceValue(context);

        return new Value2(value);
    });

    public static Task<Result<Value2>> TestAsync(Task<Result<Value1>> result) => Result<Value2>.Try(async context =>
    {
        var value = await result.ForceValue(context);

        await AsyncTask();

        return new Value2(value);
    });

    private static Task AsyncTask()
    {
        return Task.CompletedTask;
    }

    public static Result<Value2> Test2(Result<Value1> result)
    {
        return result.Match<Result<Value2>>(v => new Value2(v), e => e);
    }
}