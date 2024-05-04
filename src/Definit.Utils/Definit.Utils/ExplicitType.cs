namespace Definit.Utils;

public static class DefinitType
{
    public static string GetTypeVerboseName(this Type type)
    {
        return type.IsGenericType ? $"{type.Name[..^2]}<{string.Join(",", type.GenericTypeArguments.Select(GetTypeVerboseName))}>" : type.Name;
    }
    
    public static string GetTypeVerboseName<TType>()
    {
        return typeof(TType).GetTypeVerboseName();
    }

    public static Task<T> AsTask<T>(this T value) => Task.FromResult(value);    
}