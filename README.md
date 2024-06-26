# Definit

[![Release Status](https://img.shields.io/github/actions/workflow/status/HubiBoar/Definit/publish.yml)](https://github.com/HubiBoar/Definit/actions/workflows/publish.yml)

**Definit** Is a set of libraries aiming to help streamline working on common tasks such as Options/Configuration, Endpoints, Dependencies, Primitives, Errors/Results, Validation etc.

## Definit.Result

[![NuGet Version](https://img.shields.io/nuget/v/Definit.Result)](https://www.nuget.org/packages/Definit.Result/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Definit.Result)](https://www.nuget.org/packages/Definit.Result/)

**Definit.Result** Is a library aiming to help with exception handling by using a Result/Error pattern with a easy to use API.

#### Method returns result
```csharp
private static Result<string> HelloWorld(bool isError)
{
    try
    {
        if(isError)
        {
            return new Error("Hello Error");
        }
    
        return "Hello World";
    }
    catch(Exception exception)
    {
        return exception;
    }
}

private static async Task<Result<string>> HelloWorldAsync(bool isError)
{
    try
    {
        if(isError)
        {
            return new Error("Hello Error");
        }
    
        return "Hello World";
    }
    catch(Exception exception)
    {
        return exception;
    }
}
```

#### One liner approach
```csharp
private static void Example()
{
  if(HelloWorld(true)
      .Is(out Error error)
      .Else(out var success))
  {
     logger.LogError(error.Message); 
  }

  logger.LogInformation(success);
}

private static Task ExampleAsync()
{
  if((await HelloWorldAsync(true))
      .Is(out Error error)
      .Else(out var success))
  {
     logger.LogError(error.Message); 
  }

  logger.LogInformation(success);
}
```

#### Variable approach
```csharp
private static void Example()
{
  var result = HelloWorld(true);

  if(result
      .Is(out Error error)
      .Else(out var success))
  {
     logger.LogError(error.Message); 
  }

  logger.LogInformation(success);
}

private static async Task ExampleAsync()
{
  var result = await HelloWorldAsync(true);

  if(result
      .Is(out Error error)
      .Else(out var success))
  {
     logger.LogError(error.Message); 
  }

  logger.LogInformation(success);
}
```

#### Different result Types
```csharp
private static Result Example(bool isError)
{
    if(isError)
    {
        return new Error("Hello Error");
    }

    return Result.Success;
}

private static Result.Or<string> Example(int value)
{
    if(value == -1)
    {
        return new Error("Hello Error");
    }
    else if(value == 1)
    {
        return "string";
    }

    return Result.Success;
}

private static Result<string, int> Example(int value)
{
    if(value == -1)
    {
        return new Error("Hello Error");
    }
    else if(value == 1)
    {
        return "string";
    }

    return 5;
}
```

## [Definit.Validation](src/Definit.Validation/Definit.Validation.Tests.Unit/Fluent/Classes.cs)

[![NuGet Version](https://img.shields.io/nuget/v/Definit.Validation)](https://www.nuget.org/packages/Definit.Validation/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Definit.Validation)](https://www.nuget.org/packages/Definit.Validation/)

**Definit.Validation** Is a library aiming to help adding Validation to classes, it also supports FluentValidation out of the box.

```csharp
public sealed class DefaultValidation : IValidate<string>
{
    public static ValidationResult Validate(Validator<string> context)
    {
        if(context.Value.Length > 2)
        {
            return ValidationResult.Success;
        }

        return new ValidationErrors("Length <= 2");
    }
}

public sealed class FluentValidation : IValidate<string>
{
    public static ValidationResult Validate(Validator<string> context)
    {
        return context.FluentRule(r => r.NotEmpty().MinimumLength(3).MaximumLength(3));
    }
}

internal class FluentValidationClass : IValidate<ExampleClass>
{
    public required string Value0 { get; init; }
    
    public required string Value1 { get; init; }

    public required int Value2 { get; init; }

    public static ValidationResult Validate(Validator<ExampleClass> context)
    {
        return context.Fluent(validator =>
        {
            validator.RuleFor(x => x.Value0).NotEmpty();

            validator.RuleFor(x => x.Value1).NotEmpty();

            validator.RuleFor(x => x.CommaArray).NotNull();
        });
    }
}

private static void Check(FluentValidationClass validate)
{
    if(validate
        .IsValid(out var validValue)
        .Else(out var errors))
    {
        Run(validValue);
    }

    if(errors
        .Is(out ValidationErrors validationErrors)
        .Else(out Error error))
    {
        //print validationErrors
    }

    //print error;
}

private static void Run(Valid<FluentValidationClass> valid)
{
    //only accepts validated values
}
```

## [Definit.Primitives](src/Definit.Primitives/Definit.Primitives.Tests.Unit/ExampleClass.cs)

[![NuGet Version](https://img.shields.io/nuget/v/Definit.Primitives)](https://www.nuget.org/packages/Definit.Primitives/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Definit.Primitives)](https://www.nuget.org/packages/Definit.Primitives/)

**Definit.Primitives** Is a library aiming to help working with Primitives, making them Validatable by default.

```csharp

public sealed record IsConnectionString : ValidationMethod<IsConnectionString, string>
{
    protected override ValidationResult Validation(Validator<string> context)
    {
        return context.FluentRule(r => r.NotEmpty().MinimumLength(4));
    }
}

public sealed record IsEmail : ValidationMethod<IsEmail, string>
{
    protected override ValidationResult Validation(Validator<string> ruleBuilder)
    {
        return ruleBuilder.FluentRule(r => r.NotEmpty().MinimumLength(4));
    }
}

internal class ExampleClass : IValidate<ExampleClass>
{
    public required Value<string, IsConnectionString> ConnectionString { get; init; }
    
    public required Value<string, IsEmail> Email { get; init; }

    public static ValidationResult Validate(Validator<ExampleClass> context)
    {
        return context.Fluent(validator =>
        {
            validator.RuleFor(x => x.ConnectionString).ValidateSelf();

            validator.RuleFor(x => x.Email).ValidateSelf();
        });
    }
}

private static void Check(string validate)
{
    if(validate
        .IsValid<string, IsConnectionString>(out var validValue)
        .Else(out var errors))
    {
        Run(validValue);
    }

    if(errors
        .Is(out ValidationErrors validationErrors)
        .Else(out Error error))
    {
        //print validationErrors
    }

    //print error;
}
```

## [Definit.Configuration](src/Definit.Configuration/Definit.Configuration/Example.cs)

[![NuGet Version](https://img.shields.io/nuget/v/Definit.Configuration)](https://www.nuget.org/packages/Definit.Configuration/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Definit.Configuration)](https://www.nuget.org/packages/Definit.Configuration/)

**Definit.Configuration** Is a library helping to work with Configuration, making them Validatable by default.

#### Value
```csharp
private sealed class Value : ConfigValue<Value, string, IsConnectionString>
{
    protected override string SectionName => "Name";
}
```

#### Section
```csharp
private sealed class Section : ConfigSection<Section>
{
    protected override string SectionName { get; } = "ExampleConfigSection";

    public string Value1 { get; } = string.Empty;
    
    public string Value2 { get; } = string.Empty;

    protected override ValidationResult Validate(Validator<Section> context)
    {
        return context.Fluent(validator =>
        {
            validator.RuleFor(x => x.Value1).IsConnectionString();

            validator.RuleFor(x => x.Value2).IsConnectionString();
        });
    }
}
```

#### Feature
```csharp
private sealed class Feature : IFeatureName
{
    public static string FeatureName => "Test";
}
```

#### Inject as Dependency
```csharp
private record Dependnecy(Section.Get Section, Value.Get Value, FeatureToggle<Feature>.Get Feature)
{
  private static async Task Get()
  {
      var section = Section();
      var value = Value();
      var isEnabled = await Feature();
  }
}
```

#### DI Registration
```csharp
private static void Register(IServiceCollection services, IConfiguration configuration)
{
    services.Register<Section>(configuration);
    Value.Register(services, configuration);
    FeatureToggle<Feature>.Register(services, configuration);
}
```

#### Manual creation
```csharp
private static async Task Create(IServiceCollection services, IConfiguration configuration)
{
    var section = Section.Create(configuration);
    var value = Value.Create(configuration);
    var isEnabled = await FeatureToggle<Feature>.Create(services);
}
```


## Definit.Dependencies

[![NuGet Version](https://img.shields.io/nuget/v/Definit.Dependencies)](https://www.nuget.org/packages/Definit.Dependencies/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Definit.Dependencies)](https://www.nuget.org/packages/Definit.Dependencies/)

**Definit.Dependencies** Is a library helping to work with Dependencies, similar to how MinimalApi works.

```csharp
private static Result Handle(FromServices<Dependency1, Dependency2> dependencies)
{
    var (dep1, dep2) = dependencies;

    return Result.Success;
}

private static void Run(IServiceProvider provider)
{
    Handle(provider.From());
}
```


## Definit.Endpoint

[![NuGet Version](https://img.shields.io/nuget/v/Definit.Endpoint)](https://www.nuget.org/packages/Definit.Endpoint/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Definit.Endpoint)](https://www.nuget.org/packages/Definit.Endpoint/)

**Definit.Endpoint** Is a library helping to work with MinimalApi.

```csharp
private static Endpoint Endpoint => Map.Get("test", (int age) => 
{
    return Results.Ok();
});
```

## [Definit.Json](https://github.com/HubiBoar/Definit/blob/main/src/Definit.Primitives/Definit.Primitives.Tests.Unit/NewtonsoftTests.cs)

[![NuGet Version](https://img.shields.io/nuget/v/Definit.Json)](https://www.nuget.org/packages/Definit.Json/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/Definit.Json)](https://www.nuget.org/packages/Definit.Json/)

**Definit.Json** Is a library helping to work with Json, by forcing objects to describe how they should be Serialized.

```csharp
[SystemJsonStaticConverter]
public sealed record Example(string Value) : IJsonStaticConvertable<Example>
{
    public static string ToJson(Example value) => JsonConvert.SerializeObject(value!.Value);
    public static bool CanConvert(Type type)   => type == typeof(Example);

    public static Example FromJson(string json)
    {
        var value = JsonConvert.DeserializeObject<string>(json)!;

        return new Example(value);
    }
}
```

## License

The code in this repo is licensed under the [MIT](LICENSE) license.
