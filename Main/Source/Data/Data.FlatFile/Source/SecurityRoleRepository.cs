using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Data.Shared;
using Mud.Engine.Runtime.Game.Character;

namespace Mud.Data.FlatFile
{
    public class SecurityRoleRepository : ISecurityRoleRepository
    {
        private IFileStorageService fileService;

        public SecurityRoleRepository(IFileStorageService storageService)
        {
            this.fileService = storageService;
        }

        public async Task<IEnumerable<ISecurityRole>> GetSecurityRoles()
        {
            IEnumerable<string> roleFiles = this.fileService.GetAllFilesByExtension("role", "Security");
            var roleTasks = new List<Task<string>>();
            foreach (string file in roleFiles)
            {
                roleTasks.Add(this.fileService.LoadValueFromKeyAsync(file, "Name"));
            }

            await Task.WhenAll(roleTasks);
            return roleTasks.Select(task => new SecurityRole(task.Result, Enumerable.Empty<ISecurityPermission>()));
        }
    }
}
