using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Data.Shared
{
    public interface ISecurityRoleRepository : IRepository
    {
        Task<IEnumerable<ISecurityRole>> GetSecurityRoles();
    }
}
