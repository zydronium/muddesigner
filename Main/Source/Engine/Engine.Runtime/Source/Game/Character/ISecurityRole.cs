using System.Collections.Generic;

namespace Mud.Engine.Runtime.Game.Character
{
    public interface ISecurityRole
    {
        string Name { get; }

        IEnumerable<ISecurityPermission> Permissions { get; }
    }
}
