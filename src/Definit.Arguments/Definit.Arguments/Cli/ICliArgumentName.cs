namespace Definit.Arguments.Cli;

public interface ICliArgumentName
{
    public static abstract string Name { get; }

    public static abstract string Shortcut { get; }
}

public interface ICliArgument
{
    protected string[] Arguments { get; }
}