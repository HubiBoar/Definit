using System.Text.Json;

namespace Explicit.Primitives;

internal class SystemJsonStaticConverter : System.Text.Json.Serialization.JsonConverter<object>
{
    public override void Write(Utf8JsonWriter writer, object obj, JsonSerializerOptions options)
    {
        var value = IJsonStaticConvertable.ToJsonInternal(obj);

        writer.WriteRawValue(value);
    }

    public override object? Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
    {
        var json = JsonDocument.ParseValue(ref reader).RootElement.GetRawText();
        return IJsonStaticConvertable.FromJsonInternal(objectType, json);
    }

    public override bool CanConvert(Type objectType)
    {
        return IJsonStaticConvertable.CanConvertInternal(objectType);
    }
}
//
// internal class ValueSystemJsonConverter : System.Text.Json.Serialization.JsonConverter<object>
// {
//     public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
//     {
//         var valueType = value.GetType().GetGenericArguments()[0];
//         JsonSerializer.Serialize(writer, value.ToString(), valueType, options);
//     }
//     
//     public override object Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
//     {
//         var valueType = objectType.GetGenericArguments()[0];
//         var root = JsonDocument.ParseValue(ref reader).RootElement.ToString();
//         //var converted = Newtonsoft.Json.JsonConvert.SerializeObject(root);
//         var json = JsonDocument.ParseValue(ref reader).RootElement.Clone().Deserialize(valueType);
//         var value = Convert.ChangeType(json, valueType);
//         
//         return Convert.ChangeType(value!, objectType);
//     }
//
//     public override bool CanConvert(Type objectType)
//     {
//         return ValueHelper.CompareTypes(objectType);
//     }
// }