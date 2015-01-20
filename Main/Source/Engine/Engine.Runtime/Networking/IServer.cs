//-----------------------------------------------------------------------
// <copyright file="IServer.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Networking
{
    using Mud.Engine.Runtime.Game;
    using Mud.Engine.Runtime.Game.Character;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides a contract for objects wanting to implement a server.
    /// </summary>
    public interface IServer<TGame> : IServer where TGame : IGame, new()
    {
        /// <summary>
        /// Gets the game.
        /// </summary>
        TGame Game { get; }

        /// <summary>
        /// Starts the server using the specified game.
        /// </summary>
        /// <typeparam name="TConfiguration">The type of the server configuration.</typeparam>
        /// <returns>Returns an awaitable Task</returns>
        Task Start<TConfiguration>() where TConfiguration : IServerConfiguration, new();
    }

    public interface IServer
    {
        /// <summary>
        /// Occurs when a player connects to the server.
        /// </summary>
        event EventHandler<ServerConnectionEventArgs> PlayerConnected;

        /// <summary>
        /// Occurs when a player disconnects from the server.
        /// </summary>
        event EventHandler<ServerConnectionEventArgs> PlayerDisconnected;

        /// <summary>
        /// Gets a collection of current user connections.
        /// </summary>
        ICollection<IPlayer> ConnectedPlayers { get; }

        /// <summary>
        /// Gets or sets the port that the server is running on.
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Gets or sets the maximum connections.
        /// </summary>
        int MaxConnections { get; set; }

        /// <summary>
        /// Gets or sets the maximum queued connections.
        /// </summary>
        int MaxQueuedConnections { get; set; }

        /// <summary>
        /// Gets or sets the minimum size of the password.
        /// </summary>
        int MinimumPasswordSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the password.
        /// </summary>
        int MaximumPasswordSize { get; set; }

        /// <summary>
        /// Gets or sets the message of the day.
        /// </summary>
        ICollection<string> MessageOfTheDay { get; set; }

        /// <summary>
        /// Gets or sets the server owner.
        /// </summary>
        string Owner { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IServer"/> is enabled.
        /// </summary>
        bool IsEnabled { get; }

        /// <summary>
        /// Gets the current server status.
        /// </summary>
        ServerStatus Status { get; }

        /// <summary>
        /// Starts the server using the specified game.
        /// </summary>
        /// <param name="game">The game instance.</param>
        /// <param name="configuration">The configuration instance.</param>
        /// <returns>
        /// Returns an awaitable Task
        /// </returns>
        Task Start(IGame game, IServerConfiguration configuration);

        /// <summary>
        /// Gets the current game.
        /// </summary>
        /// <returns>Returns the currently running game on the server.</returns>
        IGame GetCurrentGame();

        /// <summary>
        /// Stops the server.
        /// </summary>
        void Stop();

        /// <summary>
        /// Disconnects the specified IServerPlayer object.
        /// </summary>
        /// <param name="player">The player.</param>
        void Disconnect(IPlayer player);

        /// <summary>
        /// Disconnects everyone from the server..
        /// </summary>
        void DisconnectAll();
    }
}
