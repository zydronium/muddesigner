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

        private Dictionary<ICharacter, IInputCommand> characterCommandStates;

        public CommandManager(IEnumerable<ISecurityRole> availableRoles, IEnumerable<IInputCommand> commands)
        {
            this.serverRoles = availableRoles ?? Enumerable.Empty<ISecurityRole>();
            this.commandCollection = commands;

            this.characterCommandStates = new Dictionary<ICharacter, IInputCommand>();
        }

        public event EventHandler<CommandCompletionArgs> CommandCompleted;

        public async Task ProcessCommandForCharacter(ICharacter character, string command)
        {
            IInputCommand currentCommand = null;
            InputCommandResult result = null;
            string[] args = command.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            bool hasKey = this.characterCommandStates.TryGetValue(character, out currentCommand);

            // if we are resuming from a previous command state, we send all of the command contents in to the current command.
            if (hasKey)
            {
                result = currentCommand.Execute(character, command, args);
            }
            else if (args.Any())
            {
                // If we are not resuming from a previous state, we find a command that maps to the first word given to us
                // in our command string param.
                currentCommand = this.commandCollection.FirstOrDefault(c => c.Command.ToLower().Equals(args.First().ToLower()));
                if (currentCommand == null)
                {
                    result = new InputCommandResult("Unknown Command.\r\n>>:", true, null, character);
                }

                else
                {
                    // We execute the command, skiping the first arg as it is the command word itself.
                    result = currentCommand.IsAsyncCommand ?
                        await currentCommand.ExecuteAsync(character, args.First(), args.Skip(1).ToArray()) :
                        currentCommand.Execute(character, args.First(), args.Skip(1).ToArray());
                }
            }
            else
            {
                result = new InputCommandResult("Unknown Command.", true, null, character);
            }

            this.UpdateCommandState(result);

            var handler = this.CommandCompleted;
            if (handler == null)
            {
                return;
            }

            handler(this, new CommandCompletionArgs(command, character, Enumerable.Empty<string>(), result));
        }

        public Task ProcessCommandForCharacter(ICharacter character, IInputCommand command)
        {
            InputCommandResult result = command.Execute(character, command.Command);
            this.UpdateCommandState(result);

            var handler = this.CommandCompleted;
            if (handler == null)
            {
                return Task.FromResult(0);
            }

            handler(this, new CommandCompletionArgs(command.Command, character, Enumerable.Empty<string>(), result));
            return Task.FromResult(0);
        }

        private void UpdateCommandState(InputCommandResult commandResult)
        {
            if (commandResult == null)
            {
                return;
            }

            IInputCommand command = null;
            bool hasKey = this.characterCommandStates.TryGetValue(commandResult.Executor, out command);
            if (!commandResult.IsCommandCompleted)
            {
                if (hasKey)
                {
                    this.characterCommandStates[commandResult.Executor] = command;
                }
                else
                {
                    this.characterCommandStates.Add(commandResult.Executor, commandResult.CommandExecuted);
                }
            }
            else
            {
                if (hasKey)
                { 
                    this.characterCommandStates.Remove(commandResult.Executor);
                }
            }
        }

        private IEnumerable<IInputCommand> SetInitialCharacterCommands(ICharacter character)
        {
            // TODO: Build out command collection
            return Enumerable.Empty<IInputCommand>();
        }
    }
}
