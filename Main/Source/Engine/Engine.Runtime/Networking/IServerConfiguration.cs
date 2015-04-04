namespace Mud.Engine.Runtime.Networking
{
    using Mud.Engine.Runtime.Game;

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
        void Configure(IGame game, IServer server);
    }
}
