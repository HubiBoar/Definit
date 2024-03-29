using System.Text.Json;
using Definit.Validation.FluentValidation;
using FluentAssertions;

namespace Definit.Primitives.Tests.Unit;

public record MoreComplexType(string InternalString);

public class SystemJsonTests
{
    [Fact]
    public void String_Write()
    {
        //Arrange
        var value = new Value<string, IsConnectionString>("test");

        //Act
        var json = JsonSerializer.Serialize(value);
        
        //Assert
        json.Should().Be("\"test\"");
    }
    
    [Fact]
    public void String_Read()
    {
        //Arrange
        var json = "\"test\"";

        //Act
        string value = JsonSerializer.Deserialize<Value<string, IsConnectionString>>(json)!;
        
        //Assert
        value.Should().Be("test");
    }
    
    [Fact]
    public void MoreComplexType_Write()
    {
        //Arrange
        var value = new Value<MoreComplexType, IsNotNull<MoreComplexType>>(new MoreComplexType("test"));

        //Act
        var json = JsonSerializer.Serialize(value);
        
        //Assert
        json.Should().Be("{\"InternalString\":\"test\"}");
    }
    
    [Fact]
    public void MoreComplexType_Read()
    {
        //Arrange
        var json = "{\"InternalString\":\"test\"}";

        //Act
        MoreComplexType value = JsonSerializer.Deserialize<Value<MoreComplexType, IsNotNull<MoreComplexType>>>(json)!;
        
        //Assert
        value.InternalString.Should().Be("test");
    }
    
    [Fact]
    public void ExampleClass_Write()
    {
        //Arrange
        var value = new ExampleClass()
        {
            ConnectionString = "connectionString",
            Email = "email"
        };

        //Act
        var json = JsonSerializer.Serialize(value);
        
        //Assert
        json.Should().Be("{\"ConnectionString\":\"connectionString\",\"Email\":\"email\"}");
    }
    
    [Fact]
    public void ExampleClass_Read()
    {
        //Arrange
        var json = "{\"ConnectionString\":\"connectionString\",\"Email\":\"email\"}";

        //Act
        var value = JsonSerializer.Deserialize<ExampleClass>(json)!;
        
        //Assert
        ((string)value.ConnectionString).Should().Be("connectionString");
        ((string)value.Email).Should().Be("email");
    }
}