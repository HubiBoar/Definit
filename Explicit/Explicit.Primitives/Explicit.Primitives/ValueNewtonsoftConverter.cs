using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OneOf.Else;

namespace Explicit.Primitives;

internal class NewtonsoftStaticConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? obj, JsonSerializer serializer)
    {
        if (obj is null) return;

        var value = IJsonStaticConvertable.ToJsonInternal(obj);

        writer.WriteRawValue(value);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.Value is not null)
        {
            var json = reader.Value.ToString();
            var wrappedJson = JsonConvert.SerializeObject(json);
            return IJsonStaticConvertable.FromJsonInternal(objectType, wrappedJson);
        }
        else
        {
            var json = JToken.Load(reader).ToString();
            return IJsonStaticConvertable.FromJsonInternal(objectType, json);
        }
    }

    public override bool CanConvert(Type objectType)
    {
        return IJsonStaticConvertable.CanConvertInternal(objectType);
    }
}