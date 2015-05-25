using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character.InputCommands
{
    public class NewCharacterCreatedMessage : IMessage<ICharacter>
    {
        public NewCharacterCreatedMessage(ICharacter character)
        {
            this.Content = character;
        }

        public ICharacter Content { get; private set; }

        public object GetContent()
        {
            return this.Content;
        }
    }
}
