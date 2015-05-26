using System;
using System.Threading.Tasks;
using Mud.Engine.Runtime;
using Mud.Engine.Runtime.Game;
using Mud.Engine.Runtime.Game.Character;
using Mud.Engine.Runtime.Game.Character.InputCommands;

namespace Mud.Apps.Windows.Desktop.Server.App
{
    [CommandAlias("Login")]
    public class PlayerLoginCommand : IInputCommand
    {
        private CommandProcess currentProcessor;

        private CharacterNameRequestor nameRequestor;

        private CharacterNameProcessor nameProcessor;

        private CharacterPasswordProcessor passwordProcessor;

        private INotificationCenter notificationManager;

        public bool ExclusiveCommand { get { return true; } }

        public PlayerLoginCommand(INotificationCenter notificationManager)
        {
            this.passwordProcessor = new CharacterPasswordProcessor();
            this.nameProcessor = new CharacterNameProcessor(notificationManager, this.passwordProcessor);
            this.nameRequestor = new CharacterNameRequestor(notificationManager, this.nameProcessor);

            this.currentProcessor = this.nameRequestor;
            this.notificationManager = notificationManager;
        }

        public bool CanExecuteCommand(ICharacter owner, params string[] args)
        {
            return true;
        }

        public Task<InputCommandResult> ExecuteAsync(ICharacter owner, params string[] args)
        {
            InputCommandResult result = null;
            if (this.currentProcessor.Process(owner, this, args, out result))
            {
                this.currentProcessor = this.currentProcessor.GetNextProcessor();

                //while(autoProcess)
                //{
                //    autoProcess = this.currentProcessor.AutoProgressToNextProcessor;

                //    if (this.currentProcessor.Process(owner, this, args, out result))
                //    {
                //        this.currentProcessor = this.currentProcessor.GetNextProcessor();
                //    }
                //}

                if (result.IsCommandCompleted)
                {
                    this.notificationManager.Publish(new NewCharacterCreatedMessage("Character created.", owner));
                }

                return Task.FromResult(result);
            }
            
            return Task.FromResult(result);
        }

        private abstract class CommandProcess
        {
            private CommandProcess nextProcess;

            public CommandProcess()
            {
            }

            public CommandProcess(CommandProcess nextProcess)
            {
                this.nextProcess = nextProcess;
            }

            protected bool HasNextStep
            {
                get
                {
                    return this.nextProcess != null;
                }
            }

            public bool AutoProgressToNextProcessor { get; protected set; }

            public CommandProcess GetNextProcessor()
            {
                return this.nextProcess;
            }

            public abstract bool Process(ICharacter owner, IInputCommand command, string[] args, out InputCommandResult result);
        }

        private class CharacterNameRequestor : CommandProcess
        {
            private INotificationCenter notificationCenter;

            public CharacterNameRequestor(INotificationCenter notificationManager) : base()
            {
                this.notificationCenter = notificationManager;
            }

            public CharacterNameRequestor(INotificationCenter notificationManager, CommandProcess nextProcessor) : base(nextProcessor)
            {
                this.notificationCenter = notificationManager;
            }

            public override bool Process(ICharacter owner, IInputCommand command, string[] args, out InputCommandResult result)
            {
                this.notificationCenter.Publish(new InformationMessage("Please enter your character name: ", owner));
                result = new InputCommandResult(false, command, owner);
                return true;
            }
        }

        private class CharacterNameProcessor : CommandProcess
        {
            private INotificationCenter notificationManager;

            public CharacterNameProcessor(INotificationCenter notificationCenter)
            {
                this.notificationManager = notificationCenter;
            }

            public CharacterNameProcessor(INotificationCenter notificationCenter, CommandProcess nextProcessor) : base(nextProcessor)
            {
                this.notificationManager = notificationCenter;
            }

            public string CharacterName { get; private set; }

            public override bool Process(ICharacter owner, IInputCommand command, string[] args, out InputCommandResult result)
            {
                if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
                {
                    result = new InputCommandResult("You must enter a character name.", !this.HasNextStep, command, owner);
                    return false;
                }

                this.notificationManager.Publish(new InformationMessage("Please enter your character's password: ", owner));
                result = new InputCommandResult(false, command, owner);
                return true;
            }
        }

        private class CharacterPasswordProcessor : CommandProcess
        {
            public CharacterPasswordProcessor() { }

            public CharacterPasswordProcessor(CommandProcess nextProcessor) : base(nextProcessor) { }

            public string CharacterPassword { get; private set; }

            public override bool Process(ICharacter owner, IInputCommand command, string[] args, out InputCommandResult result)
            {
                if (args.Length == 0)
                {
                    result = new InputCommandResult("Invalid password\r\n name: ", !this.HasNextStep, command, owner);
                    return false;
                }

                result = new InputCommandResult("Login Successful\r\n", !this.HasNextStep, command, owner);
                return true;
            }
        }
    }
}
