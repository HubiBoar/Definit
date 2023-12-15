using System.Text.Json.Serialization;

namespace Explicit.Utils.Json;

[Newtonsoft.Json.JsonConverter(typeof(NewtonsoftStaticConverter))]
[JsonConverter(typeof(SystemJsonStaticConverter))]
public interface IJsonStaticConvertable
{
    static abstract string ToJson(object obj);
    
    static abstract object FromJson(string json);

    static abstract bool CanConvert(Type type);
}