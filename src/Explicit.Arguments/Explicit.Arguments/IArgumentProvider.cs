namespace Explicit.Arguments;

public interface IArgumentProvider<TValue>
{
    public TValue GetValue();
}