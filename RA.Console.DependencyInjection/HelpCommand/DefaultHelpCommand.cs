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
                    System.Console.WriteLine($"  {command.Example}{new string(' ', exampleMaxSize - command.Example.Length)} {command.Description}");
            }
            
            return 0;
        }
    }
}
