namespace Mud.Engine.Runtime.Networking
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Mud.Engine.Runtime.Game;
    using Mud.Engine.Runtime.Game.Character;


    /// <summary>
    /// Provides configuration options for a runtime server
    /// </summary>
    public interface IServerConfiguration
    {
        /// <summary>
        /// Configures the specified game.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="server">The server.</param>
        Task Configure(IGame game, IServer server);

        Task<IEnumerable<ISecurityRole>> InitializeSecurityRoles();
    }
}
