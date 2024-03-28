using OneOf.Else;
using OneOf;

namespace Definit.NewNewResults;

file class Result_1_Example
{
    public sealed record Value1;
    public sealed record Value2(Value1 Value);

    public void HandleResult()
    {
        var result = CreateValue2();
        if(result.Is(out Value2 value).Else(out var error))
        {
        }

        result.Match(v1 => {}, error => {});
    }

    public async Task HandleResultAsync()
    {
        var result = await CreateValue2Async();
        if(result.Is(out Value2 value).Else(out var error))
        {
        }

        result.Match(v1 => {}, error => {});
    }

    public Result<Value2> CreateValue2() => Result<Value2>.Try(context => 
    {
        var value1 = CreateValue1().Force(context); 
        
        return new Value2(value1);
    });

    public Result CreateResult() => Result.Try(context => 
    {
        var value1 = CreateValue1().Force(context); 
        
        return Result.Success;
    });


    public Task<Result<Value2>> CreateValue2Async() => Result<Value2>.Try(async _ => 
    {
        var value1Result = await CreateValue1Async();

        if(value1Result.Is(out Error error).Else(out var value1))
        {
            return error;
        }

        await AsyncExample();

        return new Value2(value1);
    });

    public Task<Result<Value2>> CreateValue2AsyncForce() => Result<Value2>.Try(async context => 
    {
        var value1 = await CreateValue1Async().Force(context);

        await AsyncExample();

        return new Value2(value1);
    });

    public Result<Value1> CreateValue1() => Result<Value1>.Try(c => 
    {
        return new Value1();
    });

    public Task<Result<Value1>> CreateValue1Async() => Result<Value1>.Try(async _ => 
    {
        await AsyncExample();
        return new Value1();
    });

    public Task AsyncExample()
    {
        return Task.CompletedTask;
    }
}