using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Engine.Runtime.Game.Character
{
    public interface ICommandManager
    {
        Task ProcessCommandForCharacter(ICharacter character, string command, string[] args);

        Task ProcessCommandForCharacter(ICharacter character, IInputCommand command, string[] args);
    }
}
