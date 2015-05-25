using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Engine.Runtime.Game.Character
{
    public interface ICommandManager
    {
        void SetOwner(ICharacter owningCharacter);

        Task ProcessCommandForCharacter(string command, string[] args);

        Task ProcessCommandForCharacter(IInputCommand command, string[] args);
    }
}
