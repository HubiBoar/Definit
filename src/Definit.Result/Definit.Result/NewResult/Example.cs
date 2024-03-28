namespace Definit.NewResultsOneOf;

file static class ExampleClass
{
    public record Value1;
    public record Value2;
    public record Value3;
    public record Value4;

    private static OneOf<Value1, Value2> Example()
    {
        return new Value1();
    }

    private static async Task<OneOf<Value1, Value2, Value3>> Example2(OneOf<Value3, Value1> oneOf)
    {
        var result = await oneOf.Match(v1 => CheckAsync(), v2 => Check());


        return oneOf;
    }

    private static Task<Value1> CheckAsync() 
    {
        return Task.FromResult(new Value1());
    }

    private static Value1 Check() 
    {
        return new Value1();
    }
    
    private static async Task Example3(OneOf<Value1, Value2, Value3> oneOf)
    {
        if(oneOf.Is(out Value2 value2).Else(out var value1))
        {

        }

        if(oneOf.Is(out value1).Else(out value2))
        {

        }

        await oneOf.Match(v1 => CheckAsync(), v2 => Check(), v3 => Check());
    }
}