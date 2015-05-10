using System.Collections.Generic;

namespace Mud.Engine.Runtime.Game.Character
{
    public class ISecurityRole
    {
        string Name { get; }

        IEnumerable<ISecurityPermission> Permissions { get; }
    }
}
