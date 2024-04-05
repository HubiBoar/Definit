using Definit.Results;
using FluentAssertions;

namespace Definit.Validation.Tests.Unit.Validation;

public class ValidationMethodsTests
{
    [Fact]
    public void ValidatableSuccessMethod_ShouldReturnSuccess_WhenValidateIsCalled()
    {
        //Arrange
        var validatable = new ValidatableSuccessClass();

        //Act
        var result = ValidatableSuccessClass.Validate(new Validator<ValidatableSuccessClass>(validatable));

        //Assert
        ((bool)result.Is(out Success _)).Should().BeTrue();
        ((bool)result.Is(out ValidationErrors _)).Should().BeFalse();
    }
    
    [Fact]
    public void ValidatableErrorMethod_ShouldReturnError_WhenValidateIsCalled()
    {
        //Arrange
        var validatable = new ValidatableErrorClass();

        //Act
        var result = ValidatableErrorClass.Validate(new Validator<ValidatableErrorClass>(validatable));

        //Assert
        ((bool)result.Is(out Success _)).Should().BeFalse();
        ((bool)result.Is(out ValidationErrors _)).Should().BeTrue();
    }
    
    [Fact]
    public void ValidatableSuccessMethod_ShouldBeValid_WhenValidationIsValidIsCalled()
    {
        //Arrange
        var validatable = new ValidatableSuccessClass();

        //Act
        var result = validatable.IsValid();

        //Assert
        ((bool)result.Is(out ValidatableSuccessClass value)).Should().BeTrue();
        value.Should().Be(validatable);

        ((bool)result.Is(out ValidationErrors _)).Should().BeFalse();
    }
    
    [Fact]
    public void ValidatableErrorMethod_ShouldReturnError_WhenValidationIsValidIsCalled()
    {
        //Arrange
        var validatable = new ValidatableErrorClass();

        //Act
        var result = validatable.IsValid();

        //Assert
        ((bool)result.Is(out ValidatableErrorClass _)).Should().BeFalse();
        ((bool)result.Is(out ValidationErrors errores)).Should().BeTrue();
        errores.Should().NotBeNull();
    }
}