using System.Collections.Generic;

namespace Mud.Engine.Runtime.Game.Character
{
    public interface IPlayer : ICharacter
    {
        IEnumerable<ISecurityRole> Roles { get; }
    }
}