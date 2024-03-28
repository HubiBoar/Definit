using OneOf;

namespace Definit.NewResults;

public class Result<TValue0, TValue1>
{
}

file static class Example
{
    public record Value1();
    public record Value2(Value1 Value);

    public static Result<Value2> Test(Result<Value1> result) => Result<Value2>.Try(context =>
    {
        var value = result.Force(context);

        return new Value2(value);
    });

    public static async Task<Result<OneOf<Value1, Value2>>> TestAsyncIsOneOf()
    {
        var result = await AsyncCreate();

        if(result.IsError(out var error, out var value1))
        {
            return error;
        }

        await AsyncTask();

        var result2 = await AsyncResult();

        if(result2.IsError(out error))
        {
            return error;
        }

        return new Value2(value1);
    }

    public static async Task<Result<Value2>> TestAsyncIs()
    {
        var result = await AsyncCreate();

        if(result.IsError(out var error, out var value1))
        {
            return error;
        }

        await AsyncTask();

        var result2 = await AsyncResult();

        if(result2.IsError(out error))
        {
            return error;
        }

        return new Value2(value1);
    }


    public static async Task<Result<Value2>> TestAsyncMatch()
    {
        var result = await AsyncCreate();

        return await result.Match(async value =>
        {
            await AsyncTask();

            return await AsyncResult()
                .Match<Value2>(() => new Value2(value));
        });
    }

    public static Task<Result<Value2>> TestAsyncForce() => Result<Value2>.Try(async context =>
    {
        var value2 = await AsyncCreate();

        var value = await AsyncCreate().Force(context);

        await AsyncTask();

        await AsyncResult().Force(context);

        return new Value2(value);
    });

    public static Task<Result<Value2>> TestSync(Result<Value1> result) => Result<Value2>.Try(context =>
    {
        var value = result.Force(context);

        return new Value2(value);
    });

    public static Task<Result<OneOf<Value1, Value2>>> TestAsync(Task<Result<Value1>> result) => Result<OneOf<Value1, Value2>>.Try(async context =>
    {
        var value = await result.Force(context);

        await AsyncTask();

        return new Value2(value);
    },
    error => error);

    private static Task<Result<Value1>> AsyncCreate()
    {
        return new Value1().ToResult();
    }

    private static Task<Result> AsyncResult()
    {
        return Result.Success;
    }

    private static Task AsyncTask()
    {
        return Task.CompletedTask;
    }

    public static Task<Result<Value2>> Test2(Result<Value1> result)
    {
        return result.Match<Value2>(v => new Value2(v));
    }

    public static Task<Result> Test3(Result<Value1> result)
    {
        return result;
    }
}