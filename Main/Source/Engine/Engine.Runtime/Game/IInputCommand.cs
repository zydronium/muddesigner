using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game
{
    public interface IInputCommand
    {
        bool IsAsyncCommand { get; }

        void Execute();

        Task ExecuteAsync();
    }
}
