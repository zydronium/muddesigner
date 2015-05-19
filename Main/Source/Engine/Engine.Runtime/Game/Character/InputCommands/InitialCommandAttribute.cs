using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character.InputCommands
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class InitialCommandAttribute : Attribute
    {
        public InitialCommandAttribute(bool canBeOverriden)
        {
            this.CanBeOverriden = canBeOverriden;
        }

        public bool CanBeOverriden { get; private set; }
    }
}
