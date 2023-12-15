namespace Explicit.Utils.Json;

internal class JsonStaticConvertableHelper
{
    internal static string CallToJson(object obj)
    {
        return (string)obj.GetType().GetMethod(nameof(IJsonStaticConvertable.ToJson))!.Invoke(null, new[] {obj})!;
    }
    
    internal static object CallFromJson(Type objectType, string json)
    {
        return objectType.GetMethod(nameof(IJsonStaticConvertable.FromJson))!.Invoke(null, new object?[] {json})!;
    }
    
    internal static bool CallCanConvert(Type objectType)
    {
        if (objectType.IsAssignableTo(typeof(IJsonStaticConvertable)))
        {
            return (bool)objectType.GetMethod(nameof(IJsonStaticConvertable.CanConvert))!.Invoke(null, new object?[] {objectType}) !;
        }

        return false;
    }
}