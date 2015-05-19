using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Engine.Runtime.Game.Character
{
    public interface ICommandManager
    {
        event EventHandler<CommandCompletionArgs> CommandCompleted;

        Task ProcessCommandForCharacter(ICharacter character, string command);
    }
}
