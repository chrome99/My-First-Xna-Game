using System.Collections.Generic;

namespace My_first_xna_game
{
    class CommandCollection
    {
        public static Command throwObject = new Command("throw", ThrowObject);
        private static void ThrowObject(Player player, string input)
        {
            player.ThrowObject();
        }


        public static List<Command> commandsList = new List<Command>() { throwObject };
    }

    class Command
    {
        public delegate void CommandFunction(Player player, string input);

        public CommandFunction function;
        public string name;


        public Command(string name, CommandFunction function)
        {
            this.name = name;
            this.function = function;
        }
    }
}
