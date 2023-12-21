using OneOf;
using OneOf.Types;

namespace Explicit.Cli;

public record Argument(string Prefix, string? Value);

public static class CliArgumentHelper
{
    public static TValue GetValue<TName, TValue>(this ICliArgumentProvider<TName> value)
        where TName : ICliArgument<TValue>
    {
        return GetArgument<TName>(value.Arguments).Match(
            arg => TName.GetValue(arg, TName.Convert),
            _ => TName.GetDefaultValue());
    }

    private static OneOf<Argument, NotFound> GetArgument<TName>(string[] arguments)
        where TName : ICliArgument
    {
        var name = $"--{TName.Name}";
        var shortcut = $"-{TName.Shortcut}";
        var argument = arguments.SingleOrDefault(x => x.StartsWith($"{name}") || x.StartsWith($"{shortcut}"));
        
        if (argument is null)
        {
            return new NotFound();
        }
        else
        {
            var split = argument.Split(" ");
            if (split.Length == 1)
            {
                return new Argument(split[0], null);
            }
            
            return new Argument(split[0], split[1]);
        }
    }
}