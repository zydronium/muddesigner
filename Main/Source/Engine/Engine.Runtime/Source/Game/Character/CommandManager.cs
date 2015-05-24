using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;
using Mud.Engine.Runtime.Game.Character.InputCommands;

namespace Mud.Engine.Runtime.Game.Character
{
    public class CommandManager : ICommandManager
    {
        private IEnumerable<IInputCommand> commandCollection;

        private IEnumerable<ISecurityRole> serverRoles;

        private Dictionary<ICharacter, IInputCommand> characterCommandStates;

        private INotificationCenter notificationCenter;

        public CommandManager(IEnumerable<ISecurityRole> availableRoles, IEnumerable<IInputCommand> commands, INotificationCenter notificationCenter)
        {
            this.serverRoles = availableRoles ?? Enumerable.Empty<ISecurityRole>();
            this.commandCollection = commands;
            this.notificationCenter = notificationCenter;
            this.notificationCenter.Subscribe<CommandRequestMessage>(
                async (message, subscription) => await this.ProcessCommandForCharacter(message.Sender, message.Content, message.Arguments));

            this.characterCommandStates = new Dictionary<ICharacter, IInputCommand>();
        }

        public async Task ProcessCommandForCharacter(ICharacter character, string command, string[] args)
        {
            IInputCommand currentCommand = null;
            InputCommandResult result = null;
            bool hasKey = this.characterCommandStates.TryGetValue(character, out currentCommand);

            // if we are resuming from a previous command state, we send all of the command contents in to the current command.
            if (hasKey)
            {
                // If we are in the middle of a command, we assume the command given is an argument to progress
                // the current command along, so we union the command and args.
                var commandArguments = new List<string>(args);
                commandArguments.Insert(0, command);
                result = await currentCommand.ExecuteAsync(character, commandArguments.ToArray());
                this.CompleteProcessing(currentCommand, result);
                return;
            }
            else if (string.IsNullOrEmpty(command))
            {
                return;
            }

            // If we are not resuming from a previous state, we find a command that maps to the first word given to us
            // in our command string param.
            currentCommand = this.commandCollection.FirstOrDefault(
                c => TypePool
                    .GetAttributes<CommandAliasAttribute>(c.GetType())
                    .Any(attribute => attribute.Alias.ToLower().Equals(command.ToLower())));

            if (currentCommand == null)
            {
                result = new InputCommandResult("Unknown Command.\r\n", true, null, character);
            }

            else
            {
                // We execute the command, skiping the first arg as it is the command word itself.
                result = await currentCommand.ExecuteAsync(character, args.ToArray());
            }

            this.CompleteProcessing(currentCommand, result);
        }

        private void CompleteProcessing(IInputCommand command, InputCommandResult result)
        {
            if (result == null)
            {
                throw new NullReferenceException($"{command.GetType().Name} returned a null InputCommandResult when it shouldn't have!");
            }

            this.UpdateCommandState(result);
            this.notificationCenter.Publish(new SystemMessage(result.Result));
            if (result.IsCommandCompleted)
            {
                this.notificationCenter.Publish(new SystemMessage(">>:"));
            }
        }

        public async Task ProcessCommandForCharacter(ICharacter character, IInputCommand command, string[] args)
        {
            InputCommandResult result = await command.ExecuteAsync(character, args);
            this.UpdateCommandState(result);

            this.notificationCenter.Publish(new SystemMessage(result.Result));
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
