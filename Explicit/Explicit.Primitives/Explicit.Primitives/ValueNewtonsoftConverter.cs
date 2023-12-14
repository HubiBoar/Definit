// namespace ModulR.ValueWrapper;
//
// internal class ValueNewtonsoftConverter : Newtonsoft.Json.JsonConverter
// {
//     public override void WriteJson(JsonWriter writer, object? value, Newtonsoft.Json.JsonSerializer serializer)
//     {
//         if (value is not null)
//         {
//             writer.WriteValue(value.GetType().GetProperty("Value") !.GetValue(value));
//         }
//     }
//
//     public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, Newtonsoft.Json.JsonSerializer serializer)
//     {
//         var valueType = objectType.GetInterfaces().First(x => x.GetGenericTypeDefinition() == typeof(IValueWrapper<>));
//         var type = valueType.GetGenericArguments()[0];
//         var value = reader?.Value;
//         var changedValue = Convert.ChangeType(value, type);
//         return Activator.CreateInstance(objectType, changedValue);
//     }
//
//     public override bool CanConvert(Type objectType)
//     {
//         return objectType.IsAssignableTo(typeof(IValueWrapper<>));
//     }
// }