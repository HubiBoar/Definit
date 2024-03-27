namespace Definit.NewResults;

public class Result<TValue0, TValue1>
{
}

file static class Example
{
    private record Value1();
    private record Value2();

    private record Disabled() : IError
    {
        public Error ToError()
        {
            throw new NotImplementedException();
        }
    }

    private static Result<Value3>.Error<Disabled> Get1()
    {
        throw new Exception();
    } 

    private static Result.Error<Disabled> Get2()
    {
        throw new Exception();
    }

    private static Result<Value3, Disabled> Get3()
    {
        throw new Exception();
    } 
}