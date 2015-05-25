using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character.InputCommands
{
    public static class CommandFactory
    {
        private static Func<string, IInputCommand> factory;

        public static void SetFactory(Func<string, IInputCommand> factoryMethod)
        {
            factory = factoryMethod;
        }

        public static IInputCommand CreateCommandFromAlias(string alias)
        {
            return factory(alias);
        }
    }
}
