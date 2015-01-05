using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game
{
    public class GameShutdownArgs : EventArgs
    {
        private Action<IGameComponent, string> cancelcallback;

        public GameShutdownArgs()
        {
            cancelcallback = (sender, caller) => { }; 
        }

        public GameShutdownArgs(Action<IGameComponent, string> cancelShutdownCallback)
        {
            this.cancelcallback = cancelShutdownCallback;
        }

        public void CancelShutduwn(IGameComponent sender, [CallerMemberName] string caller = "")
        {
            this.cancelcallback(sender, caller);
        }
    }
}
