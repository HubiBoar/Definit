namespace Explicit.Configuration;

public static class OneOfAsExtensions
{
    public static OneOf<T0, T1, T2> As<T0, T1, T2>(this OneOf<T0, T1> oneOf)
    {
        return oneOf.Match<OneOf<T0, T1, T2>>(t0 => t0, t1 => t1);
    }
}