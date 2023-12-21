using FluentAssertions;

namespace Explicit.Cli.Tests.Unit;

public class CliValueArgumentsTests
{
    private class FirstArgument : ICliValueStringArgument
    {
        public static string Name { get; } = "first";
        public static string Shortcut { get; } = "f";
    }

    private class SecondArgument : ICliValueStringArgument
    {
        public static string Name { get; } = "second";
        public static string Shortcut { get; } = "s";
    }

    private class ExampleArguments : ICliArgumentProvider<FirstArgument>, ICliArgumentProvider<SecondArgument>
    {
        public string[] Arguments { get; }

        public ExampleArguments(params string[] arguments)
        {
            Arguments = arguments;
        }
    }
    
    [Fact]
    public void FullNames()
    {
        //Arrange
        var example = new ExampleArguments("--first FirstValue", "--second SecondValue");
        
        //Act
        var firstArgument = example.GetValue<FirstArgument, string>();
        var secondArgument = example.GetValue<SecondArgument, string>();

        //Assert
        firstArgument.Should().Be("FirstValue");
        secondArgument.Should().Be("SecondValue");
    }
    
    [Fact]
    public void ShortCuts()
    {
        //Arrange
        var example = new ExampleArguments("-f FirstValue", "-s SecondValue");
        
        //Act
        var firstArgument = example.GetValue<FirstArgument, string>();
        var secondArgument = example.GetValue<SecondArgument, string>();

        //Assert
        firstArgument.Should().Be("FirstValue");
        secondArgument.Should().Be("SecondValue");
    }
    
    [Fact]
    public void NotProvidedFirst()
    {
        //Arrange
        var example = new ExampleArguments("-s SecondValue");
        
        //Act
        var firstArgument = example.GetValue<FirstArgument, string>();
        var secondArgument = example.GetValue<SecondArgument, string>();

        //Assert
        firstArgument.Should().BeEmpty();
        secondArgument.Should().Be("SecondValue");
    }
    
    [Fact]
    public void NotProvidedSecond()
    {
        //Arrange
        var example = new ExampleArguments("--first FirstValue");
        
        //Act
        var firstArgument = example.GetValue<FirstArgument, string>();
        var secondArgument = example.GetValue<SecondArgument, string>();

        //Assert
        firstArgument.Should().Be("FirstValue");
        secondArgument.Should().BeEmpty();
    }
}