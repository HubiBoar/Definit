// using System.Text.Json;
//
// namespace ModulR.ValueWrapper;
//
// public class ValueSystemJsonConverter : System.Text.Json.Serialization.JsonConverter<IValueWrapper>
// {
//     public override IValueWrapper Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//     {
//         var valueType = typeToConvert.GetInterfaces().First(x => x.GetGenericTypeDefinition() == typeof(IValueWrapper<>));
//         var type = valueType.GetGenericArguments()[0];
//         var value = JsonDocument.ParseValue(ref reader).RootElement.Clone().Deserialize(type);
//         var changedValue = Convert.ChangeType(value, type);
//         return (IValueWrapper)Activator.CreateInstance(typeToConvert, changedValue) !;
//     }
//
//     public override void Write(Utf8JsonWriter writer, IValueWrapper valueWrapper, JsonSerializerOptions options)
//     {
//         var valueType = valueWrapper.GetType().GetInterfaces().First(x => x.GetGenericTypeDefinition() == typeof(IValueWrapper<>));
//         var type = valueType.GetGenericArguments()[0];
//         var val = valueType.GetProperty("Value") !.GetValue(valueWrapper);
//         System.Text.Json.JsonSerializer.Serialize(writer, val, type, options);
//     }
//
//     public override bool CanConvert(Type typeToConvert)
//     {
//         var ret = typeToConvert.IsAssignableTo(typeof(IValueWrapper));
//
//         return ret;
//     }
// }