namespace Definit.Json;

[Newtonsoft.Json.JsonConverter(typeof(NewtonsoftStaticConverter))]
[SystemJsonStaticConverter]
public interface IJsonStaticConvertable<TSelf>
    where TSelf : IJsonStaticConvertable<TSelf>
{
    static abstract string ToJson(TSelf obj);
    
    static abstract TSelf FromJson(string json);
}