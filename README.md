# Definit

**Definit** Is a set of libraries aiming to help solve common tasks such as Options/Configuration, Endpoints, Dependencies, Primitives, Errors/Results, Validation etc.

## [Definit.Validation](src/Definit.Validation/Definit.Validation.Tests.Unit/Fluent/Classes.cs)

```csharp
public sealed record IsValue0 : ValidationMethod<IsValue0, string>
{
    protected override ValidationResult Validation(Validator<string> context)
    {
        return context.FluentRule(r => r.NotEmpty().MaximumLength(2));
    }
}

public sealed class IsValue1 : IValidate<string>
{
    public static ValidationResult Validate(Validator<string> context)
    {
        return context.FluentRule(r => r.NotEmpty().MinimumLength(3).MaximumLength(3));
    }
}

internal class ExampleClass : IValidate<ExampleClass>
{
    public required IsValue0.Value Value0 { get; init; }
    
    public required Value<string, IsValue1> Value1 { get; init; }

    public required Value<string, IsCommaArrayOf<IsValue0>> CommaArray { get; init; }

    public static ValidationResult Validate(Validator<ExampleClass> context)
    {
        return context.Fluent(validator =>
        {
            validator.RuleFor(x => x.Value0).ValidateSelf();

            validator.RuleFor(x => x.Value1).ValidateSelf();

            validator.RuleFor(x => x.CommaArray).ValidateSelf();
        });
    }
}
```


## Definit.Result

#### Method returns result
```csharp
private static Result<string> HelloWorld(bool isError)
{
    if(isError)
    {
        return new Error("Hello Error");
    }

    return "Hello World";
}

private static async Task<Result<string>> HelloWorldAsync(bool isError)
{
    if(isError)
    {
        return new Error("Hello Error");
    }

    return "Hello World";
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
  if(await (HelloWorldAsync(true))
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



## [Definit.Configuration](src/Definit.Configuration/Definit.Configuration/Example.cs)

[![NuGet Version](https://img.shields.io/nuget/v/Definit.Configuration)](https://www.nuget.org/packages/Definit.Configuration/)

**Definit.Configuration** Is a library helping to 

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


## [Definit.Primitives](src/Definit.Primitives/Definit.Primitives.Tests.Unit/ExampleClass.cs)


## Definit.Dependencies

```csharp
private Result Handle(Request request, FromServices<Dependency1, Dependency2> dependencies)
{
    var (dep1, dep2) = dependencies;

    return Result.Success;
}
```


## Definit.Endpoint

```csharp
private static Endpoint Endpoint => Map.Get("test", (int age) => 
{
    return Results.Ok();
});
```

## [Definit.Json](https://github.com/HubiBoar/Definit/blob/main/src/Definit.Primitives/Definit.Primitives.Tests.Unit/NewtonsoftTests.cs)

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

Definit.Utils
