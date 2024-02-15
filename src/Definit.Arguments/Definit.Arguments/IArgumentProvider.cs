namespace Definit.Arguments;

public interface IArgumentProvider<TValue>
{
    public TValue GetValue();
}