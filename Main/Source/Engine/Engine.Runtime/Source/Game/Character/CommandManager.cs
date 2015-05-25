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

        private Stack<IInputCommand> currentlyExecutingCommands;

        private INotificationCenter notificationCenter;

        private ISubscription commandRequestSubscription;

        public CommandManager(IEnumerable<ISecurityRole> availableRoles, IEnumerable<IInputCommand> commands, INotificationCenter notificationCenter)
        {
            this.serverRoles = availableRoles ?? Enumerable.Empty<ISecurityRole>();
            this.commandCollection = commands;
            this.notificationCenter = notificationCenter;
            this.currentlyExecutingCommands = new Stack<IInputCommand>();
        }

        public ICharacter Owner { get; private set; }

        public void SetOwner(ICharacter owningCharacter)
        {
            if (this.commandRequestSubscription != null)
            {
                this.commandRequestSubscription.Unsubscribe();
            }

            if (owningCharacter == null)
            {
                return;
            }

            this.Owner = owningCharacter;
            this.Owner.Deleting += this.OnOwnerDeleting;
            this.commandRequestSubscription = this.notificationCenter.Subscribe<CommandRequestMessage>(
                (message, subscription) => this.ProcessCommandForCharacter(message.Content, message.Arguments),
                message => message.Sender == this.Owner);
        }

        public Task ProcessCommandForCharacter(string command, string[] args)
        {
            if (string.IsNullOrEmpty(command))
            {
                return Task.FromResult(0);
            }

            IInputCommand currentCommand = CommandFactory.CreateCommandFromAlias(command);
            InputCommandResult commandResult = null;
            string[] commandArguments = null;

            // Only enumerate over this collection once.
            bool hasCurrentlyExecutingCommands = this.currentlyExecutingCommands.Any();

            if (currentCommand == null && !hasCurrentlyExecutingCommands)
            {
                // No command was found and we have no state, so tell the user they've entered something invalid.
                commandResult = new InputCommandResult("Unknown Command.\r\n", true, null, this.Owner);
                this.CompleteProcessing(commandResult);
            }
            else if (currentCommand != null && (!hasCurrentlyExecutingCommands || (hasCurrentlyExecutingCommands && !this.currentlyExecutingCommands.Peek().ExclusiveCommand)))
            {
                // If the command currently being executed, but not completed, is not exclusive, then run
                // the command requested by the user.
                commandArguments = args;
            }
            else if (hasCurrentlyExecutingCommands)
            {
                currentCommand = this.currentlyExecutingCommands.Pop();

                // If we are in the middle of a command, we assume the command given is an argument to progress
                // the current command along, so we union the command and args.
                commandArguments = Enumerable
                    .Concat(new string[] { command }, args)
                    .ToArray();
            }
            return this.ProcessCommandForCharacter(currentCommand, commandArguments);
        }

        public async Task ProcessCommandForCharacter(IInputCommand command, string[] args)
        {
            if (this.Owner == null)
            {
                this.SetOwner(null);
            }

            InputCommandResult result = await command.ExecuteAsync(this.Owner, args);
            this.CompleteProcessing(result);
        }

        private void CompleteProcessing(InputCommandResult result)
        {
            if (result == null)
            {
                throw new NullReferenceException($"{result.CommandExecuted.GetType().Name} returned a null InputCommandResult when it shouldn't have!");
            }

            this.EvaluateCommandState(result);
            this.notificationCenter.Publish(new InformationMessage(result.Result, this.Owner));
            if (result.IsCommandCompleted)
            {
                this.notificationCenter.Publish(new InformationMessage(">>:", this.Owner));
            }
        }

        private void EvaluateCommandState(InputCommandResult commandResult)
        {
            if (commandResult == null || commandResult.IsCommandCompleted)
            {
                return;
            }

            this.currentlyExecutingCommands.Push(commandResult.CommandExecuted);
        }

        private IEnumerable<IInputCommand> SetInitialCharacterCommands(ICharacter character)
        {
            // TODO: Build out command collection
            return Enumerable.Empty<IInputCommand>();
        }

        private Task OnOwnerDeleting(IGameComponent arg)
        {
            this.commandRequestSubscription.Unsubscribe();
            IPlayer player = (IPlayer)arg;
            player.Deleting -= this.OnOwnerDeleting;
            return Task.FromResult(0);
        }
    }
}
