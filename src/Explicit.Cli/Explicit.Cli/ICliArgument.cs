namespace Explicit.Cli;

public interface ICliArgument
{
    public static abstract string Name { get; }

    public static abstract string Shortcut { get; }
}

public interface ICliArgument<TValue> : ICliArgument
{
    public static abstract TValue Convert(string value);
    
    public static abstract TValue GetValue(Argument argument, Func<string, TValue> converter);

    public static abstract TValue GetDefaultValue();
}

public interface ICliOptionArgument : ICliArgument<bool>
{
    static bool ICliArgument<bool>.GetValue(Argument argument, Func<string, bool> converter)
    {
        return argument.Value is null || bool.Parse(argument.Value);
    }

    static bool ICliArgument<bool>.Convert(string value)
    {
        return true;
    }

    static bool ICliArgument<bool>.GetDefaultValue()
    {
        return false;
    }
}

public interface ICliValueArgument<TValue> : ICliArgument<TValue>
{
    static TValue ICliArgument<TValue>.GetValue(Argument argument, Func<string, TValue> converter)
    {
        ArgumentNullException.ThrowIfNull(argument.Value);

        return converter(argument.Value);
    }
}

public interface ICliValueStringArgument : ICliValueArgument<string>
{
    static string ICliArgument<string>.Convert(string value)
    {
        return value;
    }

    static string ICliArgument<string>.GetDefaultValue()
    {
        return string.Empty;
    }
}