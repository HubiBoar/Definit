namespace Explicit.Utils.Json;

[Newtonsoft.Json.JsonConverter(typeof(NewtonsoftStaticConverter))]
[JsonStaticConverter]
public interface IJsonStaticConvertable<TSelf>
    where TSelf : IJsonStaticConvertable<TSelf>
{
    static abstract string ToJson(TSelf obj);
    
    static abstract TSelf FromJson(string json);
}