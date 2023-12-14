// using ModulR.Extensions.FluentValidation;
// using ModulR.Extensions.FluentValidation.External;
//
// namespace ModulR.ValueWrapper;
//
// public class TestClassWithValues : IValidatable<TestClassWithValues>
// {
//     public Value<IsConnectionString> ConnectionString { get; }
//     
//     public Value<IsEmail> Email { get; }
//
//     public TestClassWithValues(string connectionString, string email)
//     {
//         ConnectionString = connectionString;
//         Email = email;
//     }
//         
//     public static void SetupValidation(Validator<TestClassWithValues> validator)
//     {
//         validator.RuleFor(x => x.ConnectionString)
//             .ValidateSelf();
//         
//         validator.RuleFor(x => x.Email)
//             .ValidateSelf();
//     }
// }