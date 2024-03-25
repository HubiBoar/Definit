using OneOf;
using OneOf.Else;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Definit.Results;

public sealed class Success
{
    public static Success Instance { get; } = new ();

    private Success()
    {
    }
}

public sealed class ErrorContext
{
    internal static ErrorContext Instance { get; } = new ErrorContext();

    internal ErrorContext()
    {
    }
}

public static class ResultHelper
{
    internal static Result<T> Return<T>(Func<Result<T>> func, Func<Exception, Error> onException)
    {
        try
        {
            return func();
        }
        catch (ErrorException error)
        {
            return error.Error;
        }
        catch (Exception exception)
        {
            return onException(exception);
        }
    }

    internal static async Task<Result<T>> Return<T>(Func<Task<Result<T>>> func, Func<Exception, Error> onException)
    {
        try
        {
            return await func();
        }
        catch (ErrorException error)
        {
            return error.Error;
        }
        catch (Exception exception)
        {
            return onException(exception);
        }
    }

    internal static Result<T> Return<T>(Func<ErrorContext, Result<T>> func, Func<Exception, Error> onException) => Return(() => func(ErrorContext.Instance), onException);
    internal static Task<Result<T>> Return<T>(Func<ErrorContext, Task<Result<T>>> func, Func<Exception, Error> onException) => Return(() => func(ErrorContext.Instance), onException);

    internal static Result Return(Func<Result> func, Func<Exception, Error> onException)
    {
        try
        {
            return func();
        }
        catch (ErrorException error)
        {
            return error.Error;
        }
        catch (Exception exception)
        {
            return onException(exception);
        }
    }

    internal static async Task<Result> Return(Func<Task<Result>> func, Func<Exception, Error> onException)
    {
        try
        {
            return await func();
        }
        catch (ErrorException error)
        {
            return error.Error;
        }
        catch (Exception exception)
        {
            return onException(exception);
        }
    }

    internal static Result Return(Func<ErrorContext, Result> func, Func<Exception, Error> onException) => Return(() => func(ErrorContext.Instance), onException);
    internal static Task<Result> Return(Func<ErrorContext, Task<Result>> func, Func<Exception, Error> onException) => Return(() => func(ErrorContext.Instance), onException);

   
    public static async Task<T> ForceValue<T>(this Task<Result<T>> task, ErrorContext context)
    {
        var result = await task;

        return result.Match(value => value, error => throw error.ToException());
    }

    public static T ForceValue<T>(this Result<T> result, ErrorContext context)
    {
        return result.Match(value => value, error => throw error.ToException());
    }

    public static async Task<T> ForceValue<T>(this Task<Result<T>> task, ErrorContext context, Func<Error, Error> modifyError)
    {
        var result = await task;

        return result.Match(value => value, error => throw modifyError(error).ToException());
    }

    public static T ForceValue<T>(this Result<T> result, ErrorContext context, Func<Error, Error> modifyError)
    {
        return result.Match(value => value, error => throw modifyError(error).ToException());
    }

    public static async Task<T> ForceValue<T>(this Task<Result<T>> task, ErrorContext context, Error error)
    {
        var result = await task;

        return result.Match(value => value, _ => throw error.ToException());
    }

    public static T ForceValue<T>(this Result<T> result, ErrorContext context, Error error)
    {
        return result.Match(value => value, _ => throw error.ToException());
    }


    public static async Task<Success> ForceValue(this Task<Result> task, ErrorContext context)
    {
        var result = await task;

        return result.Match(value => value, error => throw error.ToException());
    }

    public static Success ForceValue(this Result result, ErrorContext context)
    {
        return result.Match(value => value, error => throw error.ToException());
    }

    public static async Task<Success> ForceValue(this Task<Result> task, ErrorContext context, Func<Error, Error> modifyError)
    {
        var result = await task;

        return result.Match(value => value, error => throw modifyError(error).ToException());
    }

    public static Success ForceValue(this Result result, ErrorContext context, Func<Error, Error> modifyError)
    {
        return result.Match(value => value, error => throw modifyError(error).ToException());
    }

    public static async Task<Success> ForceValue(this Task<Result> task, ErrorContext context, Error error)
    {
        var result = await task;

        return result.Match(value => value, _ => throw error.ToException());
    }

    public static Success ForceValue(this Result result, ErrorContext context, Error error)
    {
        return result.Match(value => value, _ => throw error.ToException());
    }

    public static Results<Ok, BadRequest<string>> OkOrBadRequest<T>(this Result<T> result)
    {
        return result.Match<Results<Ok, BadRequest<string>>>(ok => TypedResults.Ok(), error => TypedResults.BadRequest(error.Message));
    }

    public static Results<Ok<T>, BadRequest<string>> OkOrBadRequestResult<T>(this Result<T> result)
    {
        return result.Match<Results<Ok<T>, BadRequest<string>>>(ok => TypedResults.Ok(ok), error => TypedResults.BadRequest(error.Message));
    }
}

public sealed class ErrorException : Exception
{
    public Error Error { get; }

    public ErrorException(Error error) : base($"{nameof(ErrorException)} :: {error.Message}")
    {
        Error = error;
    }
}

public sealed class Error
{
    public string StackTrace { get; } = Environment.StackTrace;
    public string Message { get; }


    public Error(string message)
    {
        Message = message;
    }

    public Error(Exception exception) : this(string.Empty, exception)
    {
    }

    public Error(string prefix, Exception exception)
    {
        if(exception is ErrorException errorException)
        {
            var error = errorException.Error;

            StackTrace = error.StackTrace ?? Environment.StackTrace;
            Message = $"{prefix} :: {error.Message}";
        }
        else
        {
           StackTrace = exception.StackTrace ?? Environment.StackTrace;
           Message = $"{prefix} :: {exception.Message}";
        }
    }

    public BadRequest<string> ToBadRequest()
    {
        return TypedResults.BadRequest(Message);
    }

    public ErrorException ToException()
    {
        return new ErrorException(this);
    }

    public static implicit operator Error(Exception exception)
    {
        return new Error(exception);
    }
}

public partial class Result : Result<Success>
{
    public static Result Success { get; } = new Result(Results.Success.Instance); 

    public Result(Success value) : base(value)
    {
    }

    public Result(Error error) : base(error)
    {
    }
    
    public Result(OneOf<Success, Error> oneOf) : base(oneOf)
    {
    }

    public Result(OneOfBase<Success, Error> oneOf) : base(oneOf)
    { 
    }

    public static Result Create<T>(OneOf<T, Error> oneOf)
    {
        return new Result(oneOf.Match<OneOf<Success, Error>>(_ => Results.Success.Instance, e => e));
    }

    public static Result Create<T>(OneOfBase<T, Error> oneOf)
    {
        return new Result(oneOf.Match<OneOf<Success, Error>>(_ => Results.Success.Instance, e => e));
    }
    
    public static implicit operator Result(Success success)
    {
        return new Result(success);
    }

    public static implicit operator Result(Error error)
    {
        return new Result(error);
    }

    public static implicit operator Result(Exception exception)
    {
        return new Result(exception);
    }

    public static implicit operator Result(OneOf<Success, Error> oneOf)
    {
        return new Result(oneOf);
    }
}

public partial class Result<T> : OneOfBase<T, Error>
{
    public T? SuccessValue { get; }
    public Error? ErrorValue { get; }
    public bool Successful { get; }

    public Result(T value) : base(value)
    {
        SuccessValue = value;
        Successful = true;
    }

    public Result(Error error) : base(error)
    {
        ErrorValue = error;
        Successful = false;
    }

    public Result(OneOf<T, Error> oneOf) : base(oneOf)
    {
        if(oneOf.Is(out T value).Else(out var error))
        {
            SuccessValue = value;
            Successful = true;
        }
        else
        {
            ErrorValue = error;
            Successful = false;
        }
    }

    public Result(OneOfBase<T, Error> oneOf) : this(oneOf.Match<OneOf<T, Error>>(v => v, e => e))
    {
    }

    public Result<TOut> Match<TOut>(Func<T, Result<TOut>> func)
    {
        return Result<TOut>.Try(() => Match(x => func(x), error => error));
    }

    public Task<Result<TOut>> Match<TOut>(Func<T, Task<Result<TOut>>> func)
    {
        return Result<TOut>.Try(() => Match(x => func(x), error => Task.FromResult<Result<TOut>>(error)));
    }

    public OneOfElse<Error> IsSuccess()
    {
        return this.Is(out T _);
    }

    public OneOfElse<Error> IsSuccess(out T value)
    {
        return this.Is(out value);
    }

    public OneOfElse<T> IsError()
    {
        return this.Is(out Error _);
    }

    public OneOfElse<T> IsError(out Error error)
    {
        return this.Is(out error);
    }
    
    public static implicit operator Result(Result<T> result)
    {
        return Result.Create(result);
    }

    public static implicit operator Result<T>(T value)
    {
        return new Result<T>(value);
    }

    public static implicit operator Result<T>(Error error)
    {
        return new Result<T>(error);
    }

    public static implicit operator Result<T>(Exception exception)
    {
        return new Result<T>(exception);
    }

        public static implicit operator Result<T>(OneOf<T, Error> oneOf)
    {
        return new Result<T>(oneOf);
    }
}

public partial class Result
{
    public static Result Try(Func<Result> func) => ResultHelper.Return(func, ex => ex);

    public static Result Try(Func<Result> func, Func<Exception, Error> onException) => ResultHelper.Return(func, ex => onException(ex));



    public static Result Try(Func<ErrorContext, Result> func) => ResultHelper.Return(func, ex => ex);

    public static Result Try(Func<ErrorContext, Result> func, Func<Exception, Error> onException) => ResultHelper.Return(func, ex => onException(ex));



    public static Task<Result> Try(Func<Task<Result>> func) => ResultHelper.Return(func, ex => ex);

    public static Task<Result> Try(Func<Task<Result>> func, Func<Exception, Error> onException) => ResultHelper.Return(func, ex => onException(ex));


    public static Task<Result> Try(Func<ErrorContext, Task<Result>> func) => ResultHelper.Return(func, ex => ex);

    public static Task<Result> Try(Func<ErrorContext, Task<Result>> func, Func<Exception, Error> onException) => ResultHelper.Return(func, ex => onException(ex));

    

    public static Result Try(Action action) => Try(action, ex => ex);

    public static Result Try(Action action, Func<Exception, Error> onException)
    {
        try
        {
            action();

            return Success;
        }
        catch (Exception exception)
        {
            return onException(exception);
        }
    }
    

    public static Result Try(Action<ErrorContext> action) => Try(action, ex => ex);

    public static Result Try(Action<ErrorContext> action, Func<Exception, Error> onException) => Try(() => action(ErrorContext.Instance), onException);



    public static Task<Result> Try(Func<Task> func) => Try(func, ex => ex);

    public static async Task<Result> Try(Func<Task> func, Func<Exception, Error> onException)
    {
        try
        {
            await func();

            return Success;
        }
        catch(Exception exception)
        {
            return onException(exception);
        }
    }

    public static Task<Result> Try(Func<ErrorContext, Task> func) => Try(func, ex => ex);

    public static Task<Result> Try(Func<ErrorContext, Task> func, Func<Exception, Error> onException) => Try(() => func(ErrorContext.Instance), onException);
}

public partial class Result<T>
{
    public static Result<T> Try(Func<Result<T>> func) => ResultHelper.Return(func, ex => ex);

    public static Result<T> Try(Func<Result<T>> func, Func<Exception, Error> onException) => ResultHelper.Return(func, ex => onException(ex));


    public static Result<T> Try(Func<ErrorContext, Result<T>> func) => ResultHelper.Return(func, ex => ex);

    public static Result<T> Try(Func<ErrorContext, Result<T>> func, Func<Exception, Error> onException) => ResultHelper.Return(func, ex => onException(ex));



    public static Task<Result<T>> Try(Func<Task<Result<T>>> func) => ResultHelper.Return(func, ex => ex);

    public static Task<Result<T>> Try(Func<Task<Result<T>>> func, Func<Exception, Error> onException) => ResultHelper.Return(func, ex => onException(ex));


    public static Task<Result<T>> Try(Func<ErrorContext, Task<Result<T>>> func) => ResultHelper.Return(func, ex => ex);

    public static Task<Result<T>> Try(Func<ErrorContext, Task<Result<T>>> func, Func<Exception, Error> onException) => ResultHelper.Return(func, ex => onException(ex));
}