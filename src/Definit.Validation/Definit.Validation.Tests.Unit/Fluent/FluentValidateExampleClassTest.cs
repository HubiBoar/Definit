using Definit.Results;
using FluentAssertions;

namespace Definit.Validation.Tests.Unit.Fluent;

public class FluentValidateExampleClassTest
{
    [Fact]
    public void FluentValidatableExampleClass_ShouldReturnSuccess_WhenValuesAreValid()
    {
        //Arrange
        var example = new ExampleClass()
        {
            Value0 = "e",
            Value1 = "eee",
            CommaArray = "c,c,c"
        };

        //Act
        var result = ExampleClass.Validate(new Validator<ExampleClass>(example));

        //Assert
        ((bool)result.Is(out Success _)).Should().BeTrue();
        ((bool)result.Is(out ValidationErrors _)).Should().BeFalse();
    }
    
    [Fact]
    public void FluentValidatableExampleClass_ShouldReturnFailed_WhenValuesAreInvalid()
    {
        //Arrange
        var example = new ExampleClass()
        {
            Value0 = "eee",
            Value1 = "e",
            CommaArray = "ccc,c,ccc"
        };

        //Act
        var result = ExampleClass.Validate(new Validator<ExampleClass>(example));

        //Assert
        ((bool)result.Is(out Success _)).Should().BeFalse();
        ((bool)result.Is(out ValidationErrors errors)).Should().BeTrue();

        Console.WriteLine(errors.Message);
        Console.WriteLine("CJHUIJ");
        Console.WriteLine(errors.Errors["CommaArray"][0]);

        errors.Message.Should().Be("""ValidationErrors: [Value0] => The length of 'Value' must be 2 characters or fewer. You entered 3 characters. [Value1] => The length of 'Value' must be at least 3 characters. You entered 1 characters. [CommaArray] => (Index:0) The length of 'Value' must be 2 characters or fewer. You entered 3 characters. (Index:2) The length of 'Value' must be 2 characters or fewer. You entered 3 characters.""");

        errors.Errors.Count.Should().Be(3);
        errors.Errors["Value0"].Length.Should().Be(1);
        errors.Errors["Value1"].Length.Should().Be(1);
        errors.Errors["CommaArray"].Length.Should().Be(1);

        errors.Errors["Value0"][0].Should().Be("The length of 'Value' must be 2 characters or fewer. You entered 3 characters.");
        errors.Errors["Value1"][0].Should().Be("The length of 'Value' must be at least 3 characters. You entered 1 characters.");
        errors.Errors["CommaArray"][0].Should().Be("(Index:0) The length of 'Value' must be 2 characters or fewer. You entered 3 characters. (Index:2) The length of 'Value' must be 2 characters or fewer. You entered 3 characters.");
    }
}