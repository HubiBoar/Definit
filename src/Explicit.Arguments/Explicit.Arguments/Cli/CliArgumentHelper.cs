namespace Explicit.Arguments.Cli;

public static class CliArgumentHelper
{
    public static bool Exists<TName>(string[] arguments)
        where TName : ICliArgumentName
    {
        var name = $"--{TName.Name}";
        var shortcut = $"-{TName.Shortcut}";
        var value = arguments.SingleOrDefault(x => x == name || x == shortcut);
        return value is not null;
    }

    public static TValue GetValue<TValue>(this IArgumentProvider<TValue> value)
    {
        return value.GetValue();
    }
}