using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character
{
    public abstract class CharacterMessage : IMessage<string>
    {
        public CharacterMessage(string message, ICharacter targetCharacter)
        {
            this.Content = message;
            this.Target = targetCharacter;
        }

        public string Content { get; private set; }

        public ICharacter Target { get; private set; }

        public object GetContent()
        {
            return this.Content;
        }
    }
}
