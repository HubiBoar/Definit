using OneOf.Else;
using OneOf;

namespace Definit.NewNewResults;

public interface IError
{
    public Error ToError();
}

public sealed class ErrorException : Exception
{
    public Error Error { get; }

    public ErrorException(Error error) : base($"{nameof(ErrorException)} :: {error.Message}")
    {
        Error = error;
    }
}

public class Error : IError
{
    public string Message { get; }
    public string StackTrace { get; }

    public static implicit operator Task<Error>(Error value) => Task.FromResult(value);

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

    public ErrorException ToException() => new (this);

    public static implicit operator Error(Exception exception) => new (exception);
}