using RA.Console.DependencyInjection.Attributes;
using RA.Console.DependencyInjection.Args;

namespace RA.Console.DependencyInjection.Test.Args;

public class CustomArgsBuilderTests
{
    public class MyArgsBuilder : IArgsBuilder
    {
        public IDictionary<string, object> Build(string[] args)
        {
            // args: [command, John]
            var dict = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "Name", args[1] }
            };
            return dict;
        }
    }

    public class SampleCommands
    {
        [CommandWithArgsBuilder<MyArgsBuilder>("createuser")]
        public int Create([Parameter("name")] string name)
        {
            return name == "John" ? 0 : 1;
        }
    }

    [Fact]
    public async Task Uses_Custom_Args_Builder_To_Parse_Named_Args()
    {
        var args = new[] { "createuser", "John" };
        var app = new ConsoleAppBuilder(args)
            .UseDefaultHelpResources()
            .AddAssembly(typeof(SampleCommands).Assembly)
            .Build();

        var exit = await app.RunAsync();
        Assert.Equal(0, exit);
    }
}
