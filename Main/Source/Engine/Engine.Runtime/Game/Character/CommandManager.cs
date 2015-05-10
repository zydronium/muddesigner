using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Engine.Runtime.Game.Character
{
    public class CommandManager : ICommandManager
    {
        private IEnumerable<IInputCommand> commandCollection;

        private IEnumerable<ISecurityRole> serverRoles;

        public CommandManager(IEnumerable<ISecurityRole> availableRoles)
        {
            this.serverRoles = availableRoles ?? Enumerable.Empty<ISecurityRole>();
            this.commandCollection = Enumerable.Empty<IInputCommand>();
        }

        public Task Delete()
        {
            this.commandCollection = Enumerable.Empty<IInputCommand>();
            return Task.FromResult(0);
        }

        public Task Initialize()
        {
            // TODO: Build the command cache
            throw new NotImplementedException();
        }

        public Task ProcessCommandForCharacter(ICharacter character, string command)
        {
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
