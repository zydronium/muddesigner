using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character.InputCommands
{
    public class NewCharacterCreatedMessage : CharacterMessage
    {
        public NewCharacterCreatedMessage(string message, ICharacter targetCharacter)
            : base(message, targetCharacter)
        {
        }
    }
}
