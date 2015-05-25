using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Engine.Runtime.Game.Character
{
    public class InformationMessage : CharacterMessage
    {
        public InformationMessage(string message, ICharacter targetCharacter)
            : base(message, targetCharacter)
        {
        }
    }
}
