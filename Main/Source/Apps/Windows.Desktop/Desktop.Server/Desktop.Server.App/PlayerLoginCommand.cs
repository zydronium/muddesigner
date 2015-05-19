using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Apps.Windows.Desktop.Server.App
{
    public class PlayerLoginCommand : IInputCommand
    {
        private INotificationCenter notificationCenter;

        public PlayerLoginCommand(INotificationCenter notificationCenter)
        {

        }

        public bool IsAsyncCommand
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool CanExecuteCommand(ICharacter owner, string command, params string[] args)
        {
            return true;
        }

        public void Execute(ICharacter owner, string command, params string[] args)
        {
            notificationCenter.Publish(new CommandMessage(command, string.Empty));
        }

        public Task ExecuteAsync(ICharacter owner, string command, params string[] args)
        {
            throw new NotImplementedException();
        }
    }
}
