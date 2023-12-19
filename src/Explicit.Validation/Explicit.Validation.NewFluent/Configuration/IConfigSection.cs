namespace Explicit.Validation.NewFluent.Configuration;

public interface IConfigSection<TSelf> : ISectionName, IValidate<TSelf>
    where TSelf : IConfigSection<TSelf>
{
}