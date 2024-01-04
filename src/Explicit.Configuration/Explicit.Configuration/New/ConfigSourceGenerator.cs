using Explicit.Primitives;
using Explicit.Utils;
using Explicit.Validation;
using Explicit.Validation.FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.FeatureManagement;

namespace Explicit.Configuration.New;


public interface IConfigObject2
{
}

public interface IConfigHolder2<T>
    where T : IConfigObject2
{
    internal IServiceProvider ServiceProvider { get; }
    internal IConfiguration Configuration { get; }
}

public interface IConfigValue2<TValue, TValidationMethod> : IConfigObject2
    where TValue : notnull
    where TValidationMethod : IValidate<TValue>
{
    public static abstract string SectionName { get; }

    public static IsValid<Value<TValue, TValidationMethod>> GetValid<T>(IConfigHolder2<T> holder)
        where T : IConfigValue2<TValue, TValidationMethod>
    {
        var value = holder.Configuration.GetSection(T.SectionName).Get<TValue>();
        if(value is null)
        {
            return IsValid<Value<TValue, TValidationMethod>>.Error($"Section [{T.SectionName}].Get<{typeof(TValue).GetTypeVerboseName()}> is null");
        }
        return new Value<TValue, TValidationMethod>(value).IsValid();
    }
}

public sealed class ExampleConfigValue : IConfigValue2<string, IsConnectionString>
{
    public static string SectionName => "ExampleConfigValue";
}

public interface IConfigSection<TSelf> : IConfigObject2, IValidate<TSelf>
    where TSelf : IConfigSection<TSelf>
{
    public static abstract string SectionName { get; }

    public static IsValid<TSelf> GetValid<T>(IConfigHolder2<T> holder)
        where T : TSelf
    {
        var section = holder.Configuration.GetSection(T.SectionName).Get<TSelf>();
        if(section is null)
        {
            return IsValid<TSelf>.Error($"Section [{T.SectionName}].Get<{typeof(TSelf).GetTypeVerboseName()}> is null");
        }
        return section.IsValid();
    }
}

public class ExampleConfigSection : IConfigSection<ExampleConfigSection>
{
    public static string SectionName => "ExampleConfigSection";

    public static OneOf<Success, ValidationErrors> Validate(Validator<ExampleConfigSection> context)
    {
        return new Success();
    }
}

public interface IConfigFeatureFlag : IConfigObject2
{
    public static abstract string FeatureName { get; }

    public static IsValid<Value<bool, IsNotNull<bool>>> GetValid<T>(IConfigHolder2<T> holder)
        where T : IConfigFeatureFlag
    {
        var isEnabled = holder.ServiceProvider.GetRequiredService<IFeatureManager>().IsEnabledAsync(T.FeatureName).GetAwaiter().GetResult();
        return new Value<bool, IsNotNull<bool>>(isEnabled).IsValid();
    }
}

public class ExampleConfigFeatureFlag : IConfigFeatureFlag
{
    public static string FeatureName => "Feature";
}

public class ExampleDependencies
{
    public IConfigHolder2<ExampleConfigValue> Value { get; }
    public IConfigHolder2<ExampleConfigSection> Section { get; }
    public IConfigHolder2<ExampleConfigFeatureFlag> FeatureFlag { get; }

    public ExampleDependencies(IConfigHolder2<ExampleConfigValue> value, IConfigHolder2<ExampleConfigSection> section, IConfigHolder2<ExampleConfigFeatureFlag> featureFlag)
    {
        Value = value;
        Section = section;
        FeatureFlag = featureFlag;

        var validValue = value.GetValid();
        var validSection = section.GetValid();
        var validFeatureFlag = featureFlag.GetValid();
    }
}


//Should be generated automatically
public static class Extensions
{
    public static IsValid<Value<string, IsConnectionString>> GetValid(this IConfigHolder2<ExampleConfigValue> value)
    {
        return IConfigValue2<string, IsConnectionString>.GetValid(value);
    }

    public static IsValid<ExampleConfigSection> GetValid(this IConfigHolder2<ExampleConfigSection> value)
    {
        return IConfigSection<ExampleConfigSection>.GetValid(value);
    }

    public static IsValid<Value<bool, IsNotNull<bool>>> GetValid(this IConfigHolder2<ExampleConfigFeatureFlag> value)
    {
        return IConfigFeatureFlag.GetValid(value);
    }
}

