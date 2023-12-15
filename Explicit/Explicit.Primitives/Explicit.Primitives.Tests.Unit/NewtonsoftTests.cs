using Explicit.Validation.FluentValidation;
using FluentAssertions;
using Newtonsoft.Json;

namespace Explicit.Primitives.Tests.Unit;

public class NewtonsoftTests
{
    [Fact]
    public void String_Write()
    {
        //Arrange
        var value = new Value<string, IsConnectionString>("test");

        //Act
        var json = JsonConvert.SerializeObject(value);
        
        //Assert
        json.Should().Be("\"test\"");
    }
    
    [Fact]
    public void String_Read()
    {
        //Arrange
        var json = "\"test\"";

        //Act
        string value = JsonConvert.DeserializeObject<Value<string, IsConnectionString>>(json)!;
        
        //Assert
        value.Should().Be("test");
    }
    
    [Fact]
    public void MoreComplexType_Write()
    {
        //Arrange
        var value = new Value<MoreComplexType, IsNotNull<MoreComplexType>>(new MoreComplexType("test"));

        //Act
        var json = JsonConvert.SerializeObject(value);
        
        //Assert
        json.Should().Be("{\"InternalString\":\"test\"}");
    }
    
    [Fact]
    public void MoreComplexType_Read()
    {
        //Arrange
        var json = "{\"InternalString\":\"test\"}";

        //Act
        MoreComplexType value = JsonConvert.DeserializeObject<Value<MoreComplexType, IsNotNull<MoreComplexType>>>(json)!;
        
        //Assert
        value.InternalString.Should().Be("test");
    }
}