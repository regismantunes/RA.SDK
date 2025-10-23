using RA.Console.DependencyInjection.Attributes;

namespace RA.Console.DependencyInjection.Test.Attributes;

public class CommandAttributeTests
{
    public class SampleCommands
    {
        [Command("hello")] 
        public int Hello([Parameter("name")] string name = "World")
        {
            System.Console.WriteLine($"Hello, {name}!");
            return 0;
        }

        [CommandAsync("wait")] 
        public async Task<int> WaitAsync(int milliseconds = 10, CancellationToken ct = default)
        {
            await Task.Delay(milliseconds, ct);
            return 0;
        }
    }

    [Fact]
    public async Task Executes_Sync_Command_With_Default_Parameter()
    {
        var args = new[] { "hello" }; // no name provided -> uses default
        var app = new ConsoleAppBuilder(args)
            .UseDefaultHelpResources()
            .AddAssembly(typeof(SampleCommands).Assembly)
            .Build();

        var exit = await app.RunAsync();
        Assert.Equal(0, exit);
    }

    [Fact]
    public async Task Executes_Async_Command_With_CancellationToken()
    {
        var args = new[] { "wait" }; // uses default milliseconds and injected CT
        var app = new ConsoleAppBuilder(args)
            .UseDefaultHelpResources()
            .AddAssembly(typeof(SampleCommands).Assembly)
            .Build();

        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(1));
        var exit = await app.RunAsync(cts.Token);
        Assert.Equal(0, exit);
    }
}
