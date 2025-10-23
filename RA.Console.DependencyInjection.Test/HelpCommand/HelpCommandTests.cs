using RA.Console.DependencyInjection.Attributes;

namespace RA.Console.DependencyInjection.Test.HelpCommand;

public class HelpCommandTests
{
    public class Sample
    {
        [Command("cmd", Description = "Sample command", Example = "cmd")] 
        public int Run() => 0;
    }

    [Fact]
    public async Task Shows_Help_When_Empty_Args()
    {
        var app = new ConsoleAppBuilder([])
            .UseDefaultHelpResources()
            .AddAssembly(typeof(Sample).Assembly)
            .Build();

        var exit = await app.RunAsync();
        Assert.Equal(0, exit);
    }
}
