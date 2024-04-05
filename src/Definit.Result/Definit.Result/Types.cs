using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace Definit.Results;

public interface IApiLogError<T> : IApiResult<T>, IError, ILogResult
    where T : IResult
{
    Error IError.ToError() => new (Message, StackTrace, Log, () => ToApiResult());
}

public static class ApiLogErrorExtensions
{
    public static Error ToError<T>(this IApiLogError<T> error) where T : IResult => error.ToError();
}

public sealed class Success : IApiResult<Ok>
{
    public static Success Instance { get; } = new ();
    private Success() {}

    public Ok ToApiResult() => TypedResults.Ok();
}

public sealed class Disabled : IApiLogError<NotFound<string>>
{
    public string Message { get; }
    public StackTrace StackTrace { get; }

    public Disabled(string message)
    {
        Message = $"Disabled :: {message}";
        StackTrace = new StackTrace();
    }

    public NotFound<string> ToApiResult() => TypedResults.NotFound(Message);
    public void Log(ILogger logger) => logger.LogWarning(Message);
}

public sealed class NotFound : IApiLogError<NotFound<string>>
{
    public string Message { get; }
    public StackTrace StackTrace { get; }

    public NotFound(string message)
    {
        Message = $"NotFound :: {message}"; 
        StackTrace = new StackTrace();
    }

    public NotFound<string> ToApiResult() => TypedResults.NotFound(Message);
    public void Log(ILogger logger) => logger.LogWarning(Message);
}

public sealed class Null : IApiLogError<NotFound<string>>
{
    public string Message { get; }
    public StackTrace StackTrace { get; }

    public Null(string message)
    {
        Message = $"Null :: {message}"; 
        StackTrace = new StackTrace();
    }

    public NotFound<string> ToApiResult() => TypedResults.NotFound(Message);
    public void Log(ILogger logger) => logger.LogWarning(Message);
}