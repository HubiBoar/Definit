using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Explicit.Utils.Json;

internal class NewtonsoftStaticConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object? obj, JsonSerializer serializer)
    {
        if (obj is null) return;

        var value = JsonStaticConvertableHelper.CallToJson(obj);

        writer.WriteRawValue(value);
    }

    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
    {
        if (reader.Value is not null)
        {
            var json = reader.Value.ToString();
            var wrappedJson = JsonConvert.SerializeObject(json);
            return JsonStaticConvertableHelper.CallFromJson(objectType, wrappedJson);
        }
        else
        {
            var json = JToken.Load(reader).ToString();
            return JsonStaticConvertableHelper.CallFromJson(objectType, json);
        }
    }

    public override bool CanConvert(Type objectType)
    {
        return JsonStaticConvertableHelper.CallCanConvert(objectType);
    }
}