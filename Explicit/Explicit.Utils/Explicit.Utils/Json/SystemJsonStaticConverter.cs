﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Explicit.Utils.Json;

public sealed class StaticJsonConverterAttribute : JsonConverterAttribute
{
    public StaticJsonConverterAttribute() : base(typeof(SystemJsonStaticConverter))
    {
    }
}

internal class SystemJsonStaticConverter : System.Text.Json.Serialization.JsonConverter<object>
{
    public override void Write(Utf8JsonWriter writer, object obj, JsonSerializerOptions options)
    {
        var value = JsonStaticConvertableHelper.CallToJson(obj);

        writer.WriteRawValue(value);
    }

    public override object? Read(ref Utf8JsonReader reader, Type objectType, JsonSerializerOptions options)
    {
        var json = JsonDocument.ParseValue(ref reader).RootElement.GetRawText();
        return JsonStaticConvertableHelper.CallFromJson(objectType, json);
    }

    public override bool CanConvert(Type objectType)
    {
        return JsonStaticConvertableHelper.CallCanConvert(objectType);
    }
}