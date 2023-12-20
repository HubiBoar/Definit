namespace Explicit.Json;

internal class JsonStaticConvertableHelper
{
    internal static string CallToJson(object obj)
    {
        return (string)obj.GetType().GetMethod("ToJson")!.Invoke(null, new[] {obj})!;
    }
    
    internal static object CallFromJson(Type objectType, string json)
    {
        return objectType.GetMethod("FromJson")!.Invoke(null, new object?[] {json})!;
    }
    
    internal static bool CallCanConvert(Type objectType)
    {
        if (objectType.IsAssignableTo(typeof(IJsonStaticConvertable<>).MakeGenericType(objectType)))
        {
            return (bool)objectType.GetMethod("CanConvert")!.Invoke(null, new object?[] {objectType}) !;
        }

        return false;
    }
}