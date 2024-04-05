using Definit.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace Definit.Validation;

public sealed record ValidationErrors : IApiLogError<ValidationProblem>
{
    public static readonly string IgnorePropertyName = string.Empty;

    public IDictionary<string, string[]> Errors { get; }

    public StackTrace StackTrace { get; } = new ();

    public string Message { get; }
    public string Description { get; }

    public ValidationErrors(IDictionary<string, string[]> errors)
    {
        Errors = errors;
        Description = string.Join(" ", Errors.Select(x =>
        {
            var value = string.Join(" ", x.Value);

            if(x.Key != IgnorePropertyName)
            {
                return $"[{x.Key}] => {value}";
            }
            else
            {
                return value;
            }
        }));
        Message = $"ValidationErrors: {Description}";
    }
    
    public ValidationErrors(IReadOnlyCollection<ValidationErrors> errors) : this(
        errors
            .SelectMany(x => x.Errors)
            .GroupBy(x => x.Key)
            .ToDictionary(x => x.Key, x => x
                .SelectMany(y => y.Value)
                .ToArray())) {}

    public ValidationErrors((string PropertyName, string ErrorMessage)[] errors) : this(
        errors
            .GroupBy(x => x.PropertyName)
            .ToDictionary(x => x.Key, x => x
                .Select(y => y.ErrorMessage)
                .ToArray())) {}

    public ValidationErrors(string propertyName, string errorMessage) : this([(propertyName, errorMessage)]) {}

    public static ValidationErrors Null(string propertyName)
    {
        return new ValidationErrors(propertyName, $"Property: {propertyName} is null");
    }

    public ValidationProblem ToApiResult()
    {
        return TypedResults.ValidationProblem(Errors);
    }

    public void Log(ILogger logger)
    {
        logger.LogError(Message);
    }
}