using FluentAssertions;

namespace Explicit.Cli.Tests.Unit;

public class CliOptionsArgumentsTests
{
    private class FirstArgument : ICliOptionArgument
    {
        public static string Name { get; } = "first";
        public static string Shortcut { get; } = "f";
    }

    private class SecondArgument : ICliOptionArgument
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
        var example = new ExampleArguments("--first", "--second");
        
        //Act
        var firstArgument = example.GetValue<FirstArgument, bool>();
        var secondArgument = example.GetValue<SecondArgument, bool>();

        //Assert
        firstArgument.Should().BeTrue();
        secondArgument.Should().BeTrue();
    }
    
    [Fact]
    public void Shortcuts()
    {
        //Arrange
        var example = new ExampleArguments("-f", "-s");
        
        //Act
        var firstArgument = example.GetValue<FirstArgument, bool>();
        var secondArgument = example.GetValue<SecondArgument, bool>();

        //Assert
        firstArgument.Should().BeTrue();
        secondArgument.Should().BeTrue();
    }
    
    [Fact]
    public void NotProvidedFirst()
    {
        //Arrange
        var example = new ExampleArguments("-s");
        
        //Act
        var firstArgument = example.GetValue<FirstArgument, bool>();
        var secondArgument = example.GetValue<SecondArgument, bool>();

        //Assert
        firstArgument.Should().BeFalse();
        secondArgument.Should().BeTrue();
    }
    
    [Fact]
    public void NotProvidedSecond()
    {
        //Arrange
        var example = new ExampleArguments("--first");
        
        //Act
        var firstArgument = example.GetValue<FirstArgument, bool>();
        var secondArgument = example.GetValue<SecondArgument, bool>();

        //Assert
        firstArgument.Should().BeTrue();
        secondArgument.Should().BeFalse();
    }
}