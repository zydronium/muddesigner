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

        private bool canProcessNonGlobalCommands;

        public CommandManager(IEnumerable<ISecurityRole> availableRoles)
        {
            this.serverRoles = availableRoles ?? Enumerable.Empty<ISecurityRole>();
            this.commandCollection = Enumerable.Empty<IInputCommand>();
        }

        public event EventHandler<CommandCompletionArgs> CommandCompleted;

        public Task ProcessCommandForCharacter(ICharacter character, string command)
        {
            var handler = this.CommandCompleted;
            if (handler == null)
            {
                return Task.FromResult(0);
            }

            handler(this, new CommandCompletionArgs(command, character, Enumerable.Empty<string>()));
            return Task.FromResult(0);
        }

        private IEnumerable<IInputCommand> SetInitialCharacterCommands(ICharacter character)
        {
            // TODO: Build out command collection
            return Enumerable.Empty<IInputCommand>();
        }
    }
}
