using FluentAssertions;

namespace Explicit.Validation.Tests.Unit.Fluent;

public class FluentValidateExampleClassTest
{
    [Fact]
    public void FluentValidatableExampleClass_ShouldReturnSuccess_WhenValuesAreValid()
    {
        //Arrange
        var example = new ExampleClass()
        {
            Value0 = "e",
            Value1 = "eee"
        };

        //Act
        var result = ExampleClass.Validate(new Validator<ExampleClass>(example));

        //Assert
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
    }
    
    [Fact]
    public void FluentValidatableExampleClass_ShouldReturnFailed_WhenValuesAreInvalid()
    {
        //Arrange
        var example = new ExampleClass()
        {
            Value0 = "eee",
            Value1 = "e"
        };

        //Act
        var result = ExampleClass.Validate(new Validator<ExampleClass>(example));
        var errors = result.AsT1;
        var errorMessages = errors.ErrorMessages.ToArray();

        //Assert
        result.IsT0.Should().BeFalse();
        result.IsT1.Should().BeTrue();

        errors.Message.Should().Be("ValidationErrors: [Value0] The length of 'Value' must be 2 characters or fewer. You entered 3 characters., [Value1] The length of 'Value' must be at least 3 characters. You entered 1 characters.");

        errorMessages.Length.Should().Be(2);
        errorMessages[0].Should().Be("[Value0] The length of 'Value' must be 2 characters or fewer. You entered 3 characters.");
        errorMessages[1].Should().Be("[Value1] The length of 'Value' must be at least 3 characters. You entered 1 characters.");
    }
}