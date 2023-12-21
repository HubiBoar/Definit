namespace Explicit.Cli;

public interface ICliArgumentProvider<TName> : ICliArgumentsProvider
    where TName : ICliArgument
{
}

public interface ICliArgumentsProvider
{
    public string[] Arguments { get; }
}