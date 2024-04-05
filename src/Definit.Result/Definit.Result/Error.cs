using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Definit.Results;

public sealed class StackTrace
{
    public string Value { get; }

    public StackTrace()
    {
        Value = Environment.StackTrace;
    }

    public StackTrace(Exception exception)
    {
        if(exception is ErrorException error)
        {
            Value = error.Error.StackTrace.Value ?? Environment.StackTrace;
        }
        else
        {
            Value = exception.StackTrace ?? Environment.StackTrace;
        }
    }

    public StackTrace(Error error)
    {
        Value = error.StackTrace.Value ?? Environment.StackTrace;
    }
}

public interface IError
{
    public string Message { get; }
    public StackTrace StackTrace { get; }

    public string GetFullMessage() => $"Error :: {GetType()} :: {Message} :: {StackTrace}"; 

    public Error ToError();
}

public static class ErrorExtensions
{
    public static string GetFullMessage(this IError error) => error.GetFullMessage();
}

public interface IApiResult<T>
    where T : IResult
{
    public T ToApiResult();
}

public interface ILogResult
{
    public void Log(ILogger logger);
}

public sealed class ErrorException : Exception
{
    public Error Error { get; }

    public ErrorException(Error error) : base($"{nameof(ErrorException)} :: {error.Message}")
    {
        Error = error;
    }
}

public sealed class Error : IApiResult<IResult>, ILogResult, IError
{
    public string Message { get; }
    public StackTrace StackTrace { get; }

    private readonly Func<IResult> _getResult;
    private readonly Action<ILogger> _logMethod;

    public Error(string message, StackTrace? stackTrace = null, Action<ILogger>? logMethod = null, Func<IResult>? getResult = null)
    {
        Message = message;
        StackTrace = stackTrace ?? new StackTrace();
        _logMethod = logMethod ?? LogDefault;
        _getResult = getResult ?? ToBadRequest;
    }

    public Error(Exception exception)
    {
        if(exception is ErrorException errorException)
        {
            var error = errorException.Error;

            StackTrace = new StackTrace(error);
            Message = error.Message;
            _getResult = error.ToApiResult;
            _logMethod = error.Log;
        }
        else
        {
           StackTrace = new StackTrace(exception);
           Message = exception.Message;

           _logMethod = LogDefault;
           _getResult= ToBadRequest;
        }
    }

    private void LogDefault(ILogger logger)  => logger.LogError(Message);
    public void Log(ILogger logger)          => _logMethod(logger);

    public BadRequest<string> ToBadRequest() => TypedResults.BadRequest(Message);
    public IResult ToApiResult()             => _getResult();

    public ErrorException ToException()      => new (this);
    public Error ToError()                   => this;

    public static implicit operator Task<Error>(Error value)   => Task.FromResult(value);
    public static implicit operator Error(Exception exception) => new (exception);
}