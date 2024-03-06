namespace Definit.Configuration;

public interface IConfigObject<TValue>
    where TValue : IValidate<TValue>
{
    public static abstract IsValid<TValue> Create(IConfiguration configuration);
}

public interface ISectionName
{
    public abstract static string SectionName {get;}
}

public static class ConfigHelper
{
    public static OneOf<TValue, ValidationErrors> GetValue<TValue, TName>(IConfiguration configuration)
        where TName : ISectionName
    {
        var section = configuration.GetSection(TName.SectionName);
        if(section.Exists() == false)
        {
            return new ValidationErrors($"Section: [{TName.SectionName}] Is Missing");
        }

        var value = section.Get<TValue>();
        if (value is null)
        {
            return ValidationErrors.Null(DefinitType.GetTypeVerboseName<TValue>());
        }

        return value;
    }
}