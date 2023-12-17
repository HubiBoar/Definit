namespace Explicit.Utils;

public static class TypeExtensions
{
    public static string GetTypeVerboseName(this Type type)
    {
        return type.IsGenericType ? $"{type.Name[..^2]}<{string.Join(",", type.GenericTypeArguments.Select(GetTypeVerboseName))}>" : type.Name;
    }
}