using System;

namespace Mud.Engine.Runtime.Game.Character.InputCommands
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class CommandAliasAttribute : Attribute
    {
        public CommandAliasAttribute(string alias)
        {
            this.Alias = alias;
        }

        public string Alias { get; private set; }
    }
}
