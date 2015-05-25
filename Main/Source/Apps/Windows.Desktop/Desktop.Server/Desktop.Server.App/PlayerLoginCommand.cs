using System;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game;
using Mud.Engine.Runtime.Game.Character;
using Mud.Engine.Runtime.Game.Character.InputCommands;

namespace Mud.Apps.Windows.Desktop.Server.App
{
    [CommandAlias("Login")]
    public class PlayerLoginCommand : IInputCommand
    {
        private LoginProcessor currentProcessor;

        private CharacterNameRequestor nameRequestor;

        private CharacterNameProcessor nameProcessor;

        private CharacterPasswordRequestor passwordRequestor;

        private CharacterPasswordProcessor passwordProcessor;

        private INotificationCenter notificationManager;

        public bool ExclusiveCommand { get { return true; } }

        public PlayerLoginCommand(INotificationCenter notificationManager)
        {
            this.passwordProcessor = new CharacterPasswordProcessor();
            this.passwordRequestor = new CharacterPasswordRequestor(this.passwordProcessor);
            this.nameProcessor = new CharacterNameProcessor(this.passwordRequestor);
            this.nameRequestor = new CharacterNameRequestor(this.nameProcessor);

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
                bool autoProcess = this.currentProcessor.AutoProgressToNextProcessor;
                this.currentProcessor = this.currentProcessor.GetNextProcessor();
                while(autoProcess)
                {
                    autoProcess = this.currentProcessor.AutoProgressToNextProcessor;

                    if (this.currentProcessor.Process(owner, this, args, out result))
                    {
                        this.currentProcessor = this.currentProcessor.GetNextProcessor();
                    }
                }

                if (result.IsCommandCompleted)
                {
                    this.notificationManager.Publish(new NewCharacterCreatedMessage("Character created.", owner));
                }

                return Task.FromResult(result);
            }
            
            return Task.FromResult(result);
        }

        private abstract class LoginProcessor
        {
            private LoginProcessor nextProcessor;

            public LoginProcessor()
            {
            }

            public LoginProcessor(LoginProcessor nextProcessor)
            {
                this.nextProcessor = nextProcessor;
            }

            protected bool HasNextStep
            {
                get
                {
                    return this.nextProcessor != null;
                }
            }

            public bool AutoProgressToNextProcessor { get; protected set; }

            public LoginProcessor GetNextProcessor()
            {
                return this.nextProcessor;
            }

            public abstract bool Process(ICharacter owner, IInputCommand command, string[] args, out InputCommandResult result);
        }

        private class CharacterNameRequestor : LoginProcessor
        {
            public CharacterNameRequestor() { }

            public CharacterNameRequestor(LoginProcessor nextProcessor) : base(nextProcessor) { }

            public override bool Process(ICharacter owner, IInputCommand command, string[] args, out InputCommandResult result)
            {
                result = new InputCommandResult("Please enter your character name: ", !this.HasNextStep, command, owner);
                return true;
            }
        }

        private class CharacterNameProcessor : LoginProcessor
        {
            public CharacterNameProcessor() { }

            public CharacterNameProcessor(LoginProcessor nextProcessor) : base(nextProcessor) { }

            public string CharacterName { get; private set; }

            public override bool Process(ICharacter owner, IInputCommand command, string[] args, out InputCommandResult result)
            {
                if (args.Length == 0 || string.IsNullOrWhiteSpace(args[0]))
                {
                    result = new InputCommandResult("You must enter a character name.", !this.HasNextStep, command, owner);
                    return false;
                }

                result = new InputCommandResult(!this.HasNextStep, command, owner);
                this.AutoProgressToNextProcessor = true;
                return true;
            }
        }

        private class CharacterPasswordRequestor: LoginProcessor
        {
            public CharacterPasswordRequestor() { }

            public CharacterPasswordRequestor(LoginProcessor nextProcessor) : base(nextProcessor) { }

            public override bool Process(ICharacter owner, IInputCommand command, string[] args, out InputCommandResult result)
            {
                result = new InputCommandResult("Please enter your character's password: ", !this.HasNextStep, command, owner);
                return true;
            }
        }

        private class CharacterPasswordProcessor : LoginProcessor
        {
            public CharacterPasswordProcessor() { }

            public CharacterPasswordProcessor(LoginProcessor nextProcessor) : base(nextProcessor) { }

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
