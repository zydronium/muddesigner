using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game.Character
{
    public class SecurityRole : ISecurityRole
    {
        public SecurityRole(string name, IEnumerable<ISecurityPermission> permissions)
        {
            this.Name = name;
            this.Permissions = permissions;
        }

        public string Name { get; private set; }

        public IEnumerable<ISecurityPermission> Permissions { get; private set; }
    }
}
