using Explicit.Validation;

namespace Explicit.Configuration;

public interface IConfigSection<TSelf> : ISectionName, IValidate<TSelf>
    where TSelf : IConfigSection<TSelf>
{
}