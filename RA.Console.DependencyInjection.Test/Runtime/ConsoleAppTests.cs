using RA.Console.DependencyInjection.Attributes;

namespace RA.Console.DependencyInjection.Test.Runtime;

public class ConsoleAppTests
{
    public class Sample
    {
        [Command("ok")]
        public int Ok() => 0;
    }

    [Fact]
    public async Task Current_Is_Set_During_Run_And_Cleared_After()
    {
        var app = new ConsoleAppBuilder(["ok"])
            .UseDefaultHelpResources()
            .AddAssembly(typeof(Sample).Assembly)
            .Build();

        Assert.Null(ConsoleApp.Current);
        var exit = await app.RunAsync();
        Assert.Equal(0, exit);
        Assert.Null(ConsoleApp.Current);
    }

    [Fact]
    public async Task Running_Unknown_Command_Fails()
    {
        var app = new ConsoleAppBuilder(["missing"])
            .UseDefaultHelpResources()
            .AddAssembly(typeof(Sample).Assembly)
            .Build();

        await Assert.ThrowsAsync<InvalidOperationException>(() => ((ConsoleApp)app).RunCommandAsync("missing", []));
    }
}
