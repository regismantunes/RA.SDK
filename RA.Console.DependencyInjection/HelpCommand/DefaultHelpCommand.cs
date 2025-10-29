namespace RA.Console.DependencyInjection.HelpCommand
{
    public class DefaultHelpCommand : IHelpCommand
    {
        public int Execute(IEnumerable<CommandInfo> commands)
        {
            System.Console.WriteLine("Usage:");
            var exampleMaxSize = commands.Max(c => c.Example.Length);
            foreach (var commandGroup in commands.GroupBy(c => c.Group)
                                                 .OrderBy(g => g.Key))
            {
                if (commandGroup.Key is not null)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine($"  {commandGroup.Key}");
                }

                foreach (var command in commandGroup.OrderBy(c => c.Order)
                                                    .ThenBy(c => c.Example))
                {
                    var helpLineExample = $"  {command.Example}{new string(' ', exampleMaxSize - command.Example.Length)} ";
                    var helpLineFull = string.Concat(helpLineExample, command.Description);
                    if (helpLineExample.Length < System.Console.WindowWidth &&
                        helpLineFull.Length > System.Console.WindowWidth)
                    {
                        var count = 1;
                        do
                        {
                            helpLineFull = helpLineFull.Insert(System.Console.WindowWidth * count, string.Concat(Environment.NewLine, new string(' ', helpLineExample.Length)));
                            count++;
                        } while(helpLineFull.Length > System.Console.WindowWidth * count);
                    }
                    System.Console.WriteLine(helpLineFull);
                }
            }
            
            return 0;
        }
    }
}
