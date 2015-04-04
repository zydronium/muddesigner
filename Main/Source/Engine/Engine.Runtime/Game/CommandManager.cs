using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Engine.Runtime.Game
{
    public class CommandManager : ICommandManager
    {
        private static IEnumerable<IInputCommand> commandCache
            = new List<IInputCommand>();
        
        private static Dictionary<ICharacter, IEnumerable<IInputCommand>> commandCollection
            = new Dictionary<ICharacter, IEnumerable<IInputCommand>>();

        public Task Delete()
        {
            commandCollection.Clear();
            return Task.FromResult(0);
        }

        public Task Initialize()
        {
            // TODO: Build the command cache
            throw new NotImplementedException();
        }

        public Task ProcessCommandForCharacter(ICharacter character, string command)
        {
            // If the character does not have any commands assigned to it, then we build out
            // a collection of commands that are permissions independent.
            IEnumerable<IInputCommand> commandsForCharacter = null;
            if (!commandCollection.TryGetValue(character, out commandsForCharacter))
            {
                commandsForCharacter = this.SetInitialCharacterCommands(character);

                // Execute the "InitialCommand" then return.
                return Task.FromResult(0);
            }

            // Look up a matching command, execute and return.
            return Task.FromResult(0);
        }

        private IEnumerable<IInputCommand> SetInitialCharacterCommands(ICharacter character)
        {
            // TODO: Build out command collection
            return Enumerable.Empty<IInputCommand>();
        }
    }
}
