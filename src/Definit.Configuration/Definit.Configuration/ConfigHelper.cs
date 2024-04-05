using Definit.Results;

namespace Definit.Configuration;

public interface IConfigObject
{
    public static abstract ValidationResult Register(IServiceCollection services, IConfiguration configuration);

    public static abstract ValidationResult ValidateConfiguration(IConfiguration configuration);
}

public interface IConfigObject<TValue> : IConfigObject
    where TValue : IValidate<TValue>
{
    public static abstract IsValid<TValue> Create(IServiceProvider services, IConfiguration configuration);
}

public interface ISectionName
{
    public abstract static string SectionName { get; }
}

public static class ConfigHelper
{
    public static Result<TValue, ValidationErrors> GetValue<TValue>(IConfiguration configuration, string sectionName)
        where TValue : notnull
    {
        try
        {
            var section = configuration.GetSection(sectionName);
            if(section.Exists() == false)
            {
                return new ValidationErrors(sectionName, $"Section: [{sectionName}] Is Missing");
            }

            var value = section.Get<TValue>();
            if (value is null)
            {
                return ValidationErrors.Null(DefinitType.GetTypeVerboseName<TValue>());
            }

            return value;
        }
        catch(Exception exception)
        {
            return exception;
        }
    }

    public static ValidationResult Register<T>(this IServiceCollection services, IConfiguration configuration)
        where T : IConfigObject
    {
        return T.Register(services, configuration);
    }
}