using Definit.Results;
using FluentAssertions;

namespace Definit.Validation.Tests.Unit.Validation;

public class ValidatableClassesTests
{
    [Fact]
    public void ValidatableSuccessClass_ShouldReturnSuccess_WhenValidateIsCalled()
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
    public void ValidatableErrorClass_ShouldReturnError_WhenValidateIsCalled()
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
    public void ValidatableSuccessClass_ShouldBeValid_WhenValidationIsValidIsCalled()
    {
        //Arrange
        var validatable = new ValidatableSuccessClass();

        //Act
        var result = validatable.IsValid();

        //Assert
        ((bool)result.Is(out ValidatableSuccessClass valid)).Should().BeTrue();
        valid.Should().Be(validatable);

        ((bool)result.Is(out ValidationErrors _)).Should().BeFalse();
    }
    
    [Fact]
    public void ValidatableErrorClass_ShouldReturnError_WhenValidationIsValidIsCalled()
    {
        //Arrange
        var validatable = new ValidatableErrorClass();

        //Act
        var result = validatable.IsValid();

        //Assert
        ((bool)result.Is(out ValidatableErrorClass _)).Should().BeFalse();

        ((bool)result.Is(out ValidationErrors errors)).Should().BeTrue();
        errors.Should().NotBeNull();
    }
}