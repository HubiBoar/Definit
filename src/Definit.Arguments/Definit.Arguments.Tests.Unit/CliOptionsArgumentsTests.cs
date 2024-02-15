using Definit.Arguments.Cli;
using FluentAssertions;

namespace Definit.Arguments.Tests.Unit;

public class CliOptionsArgumentsTests
{
    private record FirstValue(bool Value);

    private record SecondValue(bool Value);
    
    private class FirstArgument : ICliArgumentName
    {
        public static string Name { get; } = "first";
        public static string Shortcut { get; } = "f";
    }

    private class SecondArgument : ICliArgumentName
    {
        public static string Name { get; } = "second";
        public static string Shortcut { get; } = "s";
    }

    private class ExampleArguments : ICliOptionArgument<FirstArgument, FirstValue>, ICliOptionArgument<SecondArgument, SecondValue>
    {
        public string[] Arguments { get; }

        public ExampleArguments(params string[] arguments)
        {
            Arguments = arguments;
        }

        FirstValue ICliOptionArgument<FirstArgument, FirstValue>.Convert(bool value) => new FirstValue(value);

        SecondValue ICliOptionArgument<SecondArgument, SecondValue>.Convert(bool value) => new SecondValue(value);
    }
    
    [Fact]
    public void FullNames()
    {
        //Arrange
        var example = new ExampleArguments("--first", "--second");
        
        //Act
        var firstArgument = example.GetValue<FirstValue>();
        var secondArgument = example.GetValue<SecondValue>();

        //Assert
        firstArgument.Value.Should().BeTrue();
        secondArgument.Value.Should().BeTrue();
    }
    
    [Fact]
    public void Shortcuts()
    {
        //Arrange
        var example = new ExampleArguments("-f", "-s");
        
        //Act
        var firstArgument = example.GetValue<FirstValue>();
        var secondArgument = example.GetValue<SecondValue>();

        //Assert
        firstArgument.Value.Should().BeTrue();
        secondArgument.Value.Should().BeTrue();
    }
    
    [Fact]
    public void NotProvidedFirst()
    {
        //Arrange
        var example = new ExampleArguments("-s");
        
        //Act
        var firstArgument = example.GetValue<FirstValue>();
        var secondArgument = example.GetValue<SecondValue>();

        //Assert
        firstArgument.Value.Should().BeFalse();
        secondArgument.Value.Should().BeTrue();
    }
    
    [Fact]
    public void NotProvidedSecond()
    {
        //Arrange
        var example = new ExampleArguments("--first");
        
        //Act
        var firstArgument = example.GetValue<FirstValue>();
        var secondArgument = example.GetValue<SecondValue>();

        //Assert
        firstArgument.Value.Should().BeTrue();
        secondArgument.Value.Should().BeFalse();
    }
}