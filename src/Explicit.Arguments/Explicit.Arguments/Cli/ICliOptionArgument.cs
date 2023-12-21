namespace Explicit.Arguments.Cli;

public interface ICliOptionArgument<TName, TValue> : ICliArgument, IArgumentProvider<TValue>
    where TName : ICliArgumentName
{
    TValue IArgumentProvider<TValue>.GetValue()
    {
        return Convert(CliArgumentHelper.Exists<TName>(Arguments));
    }

    TValue Convert(bool value);
}