using RA.Console.DependencyInjection.Attributes;
using RA.Console.DependencyInjection.Middleware;

namespace RA.Console.DependencyInjection.Test.Middleware;

public class MiddlewareTests
{
    public class CaptureMiddleware : ICommandMiddleware
    {
        public List<string> Events { get; } = [];

        public async Task<int> InvokeAsync(CommandContext ctx, Func<CommandContext, Task<int>> next)
        {
            Events.Add($"pre:{ctx.Command}");
            var result = await next(ctx);
            Events.Add($"post:{ctx.Command}");
            return result;
        }
    }

    public class SampleCommands
    {
        [Command("ping")] public int Ping() => 0;
    }

    [Fact]
    public async Task Invokes_Middleware_Around_Command()
    {
        var middleware = new CaptureMiddleware();
        var app = new ConsoleAppBuilder(new[] { "ping" })
            .UseDefaultHelpResources()
            .UseMeddleware(middleware)
            .AddAssembly(typeof(SampleCommands).Assembly)
            .Build();

        var exit = await app.RunAsync();
        Assert.Equal(0, exit);
        Assert.Equal(new[] { "pre:ping", "post:ping" }, middleware.Events);
    }
}
