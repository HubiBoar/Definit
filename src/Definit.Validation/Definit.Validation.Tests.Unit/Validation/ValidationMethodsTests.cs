﻿using FluentAssertions;

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
        result.IsT0.Should().BeTrue();
        result.IsT1.Should().BeFalse();
    }
    
    [Fact]
    public void ValidatableErrorMethod_ShouldReturnError_WhenValidateIsCalled()
    {
        //Arrange
        var validatable = new ValidatableErrorClass();

        //Act
        var result = ValidatableErrorClass.Validate(new Validator<ValidatableErrorClass>(validatable));

        //Assert
        result.IsT0.Should().BeFalse();
        result.IsT1.Should().BeTrue();
    }
    
    [Fact]
    public void ValidatableSuccessMethod_ShouldBeValid_WhenValidationIsValidIsCalled()
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
    public void ValidatableErrorMethod_ShouldReturnError_WhenValidationIsValidIsCalled()
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