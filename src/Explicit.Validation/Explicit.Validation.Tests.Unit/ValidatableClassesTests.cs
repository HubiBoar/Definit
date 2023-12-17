using FluentAssertions;

namespace Explicit.Validation.Tests.Unit;

public class ValidatableClassesTests
{
    [Fact]
    public void ValidatableSuccessClass_ShouldReturnSuccess_WhenValidateIsCalled()
    {
        //Arrange
        var validatable = new ValidatableSuccessClass();

        //Act
        var result = validatable.Validate();

        //Assert
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
    }
    
    [Fact]
    public void ValidatableErrorClass_ShouldReturnError_WhenValidateIsCalled()
    {
        //Arrange
        var validatable = new ValidatableErrorClass();

        //Act
        var result = validatable.Validate();

        //Assert
        result.IsT0.Should().BeFalse();
        result.IsT1.Should().BeTrue();
    }
    
    [Fact]
    public void ValidatableSuccessClass_ShouldBeValid_WhenValidationIsValidIsCalled()
    {
        //Arrange
        var validatable = new ValidatableSuccessClass();

        //Act
        var result = validatable.IsValid();

        //Assert
        result.IsT0.Should().BeTrue();
        result.AsT0.ValidValue.Should().Be(validatable);

        result.IsT1.Should().BeFalse();
    }
    
    [Fact]
    public void ValidatableErrorClass_ShouldReturnError_WhenValidationIsValidIsCalled()
    {
        //Arrange
        var validatable = new ValidatableErrorClass();

        //Act
        var result = validatable.IsValid();

        //Assert
        result.IsT0.Should().BeFalse();

        result.IsT1.Should().BeTrue();
        result.AsT1.Should().NotBeNull();
    }
}