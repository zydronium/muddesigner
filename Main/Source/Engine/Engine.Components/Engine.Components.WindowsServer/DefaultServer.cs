//-----------------------------------------------------------------------
// <copyright file="DefaultServer.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Components.WindowsServer
{
    using Mud.Engine.Runtime;
    using Mud.Engine.Runtime.Game;
    using Mud.Engine.Runtime.Game.Character;
    using Mud.Engine.Runtime.Networking;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;
    using System.Text;
    using System.Diagnostics;




    /// <summary>
    /// The Default Desktop game Server
    /// </summary>
    public class DefaultServer<TGame> : IServer<TGame> where TGame : IGame, new()
    {
        /// <summary>
        /// The user connection buffer size
        /// </summary>
        private const int UserConnectionBufferSize = 1024;

        /// <summary>
        /// The server socket
        /// </summary>
        private Socket serverSocket;

        /// <summary>
        /// The player connections
        /// </summary>
        private Dictionary<IPlayer, PlayerConnectionState> playerConnections;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultServer"/> class.
        /// </summary>
        public DefaultServer()
        {
            this.ConnectedPlayers = new List<IPlayer>();
            this.MessageOfTheDay = new List<string>();
            this.Status = new ServerStatus();

            // TODO: 11/3/14 - Change the Type to ConcurrentDictionary for thread-safety.
            this.playerConnections = new Dictionary<IPlayer, PlayerConnectionState>();
        }

        /// <summary>
        /// Occurs when a player connects to the server.
        /// </summary>
        public event EventHandler<ServerConnectionEventArgs> PlayerConnected;

        /// <summary>
        /// Occurs when a player is disconnected from the server.
        /// </summary>
        public event EventHandler<ServerConnectionEventArgs> PlayerDisconnected;

        /// <summary>
        /// Gets the game.
        /// </summary>
        public TGame Game { get; private set; }

        /// <summary>
        /// Gets a collection of users currently connected.
        /// </summary>
        public ICollection<IPlayer> ConnectedPlayers { get; private set; }

        /// <summary>
        /// Gets or sets the port that the server is running on.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the maximum connections.
        /// </summary>
        public int MaxConnections { get; set; }

        /// <summary>
        /// Gets or sets the maximum queued connections.
        /// </summary>
        public int MaxQueuedConnections { get; set; }

        /// <summary>
        /// Gets or sets the minimum size of the password.
        /// </summary>
        public int MinimumPasswordSize { get; set; }

        /// <summary>
        /// Gets or sets the maximum size of the password.
        /// </summary>
        public int MaximumPasswordSize { get; set; }

        /// <summary>
        /// Gets or sets the message of the day.
        /// </summary>
        public ICollection<string> MessageOfTheDay { get; set; }

        /// <summary>
        /// Gets or sets the server owner.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IServer" /> is enabled.
        /// </summary>
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Gets the current server status.
        /// </summary>
        public ServerStatus Status { get; private set; }

        public async Task Start(IGame game, IServerConfiguration configuration)
        {
            // Set up our game instance
            ExceptionFactory.ThrowIf<InvalidCastException>(
                !(game is TGame),
                $"Unable to cast {game.GetType().Name} to {typeof(TGame).Name}");

            this.Game = (TGame)game;
            await this.Game.Initialize();

            this.Status = ServerStatus.Starting;
            //// this.Logger("Starting network server.");

            // Validate our settings.
            ExceptionFactory
                .ThrowIf<InvalidOperationException>(this.Port <= 0, "Invalid Port number used. Recommended number is 23 or 4000")
                .Or(this.MaxConnections < 2, "Invalid MaxConnections number used. Must be greater than 1.");

            // Get our server address information
            IPHostEntry serverHost = Dns.GetHostEntry(Dns.GetHostName());
            var serverEndPoint = new IPEndPoint(IPAddress.Any, this.Port);

            var config = configuration;
            config.Configure(this.Game, this);

            // Instance the server socket, bind it to a port.
            this.serverSocket = new Socket(serverEndPoint.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            this.serverSocket.Bind(serverEndPoint);
            this.serverSocket.Listen(this.MaxQueuedConnections);

            // Begin listening for connections.
            IAsyncResult result = this.serverSocket.BeginAccept(new AsyncCallback(this.ConnectClient), this.serverSocket);

            this.Status = ServerStatus.Running;
        }

        /// <summary>
        /// Starts the server for the specified game.
        /// </summary>
        /// <typeparam name="TConfiguration">The type of the server configuration.</typeparam>
        public Task Start<TConfiguration>() where TConfiguration : IServerConfiguration, new()
            => this.Start(new TGame(), new TConfiguration());

        /// <summary>
        /// Gets the current game.
        /// </summary>
        /// <returns>Returns the current IGame instance running.</returns>
        public IGame GetCurrentGame() => this.Game;

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString() => $"{this.Game.Information.Name} - {this.Game.Information.Version}";

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            // this.LogMessage("Stopping the network server.");
            this.DisconnectAll();

            // If the server socket is still connected, we shut it down.

            // We test to ensure the server socket is still connected and active.
            this.serverSocket.Blocking = false;
            try
            {
                this.serverSocket.Send(new byte[1], 0, 0);

                // Message was received meaning it's still receiving, so we can safely shut it down.
                this.serverSocket.Shutdown(SocketShutdown.Both);
            }
            catch (SocketException e)
            {
                // Error code 10035 indicates it works, but will block the socket.
                // This means it is still receiving and we can safely shut it down.
                // Otherwise, it's not receiving anything and we don't need to shut down.
                if (e.NativeErrorCode.Equals(10035))
                {
                    this.serverSocket.Shutdown(SocketShutdown.Both);
                }
            }
            finally
            {
                this.Status = ServerStatus.Stopped;
                this.IsEnabled = false;
            }
        }

        /// <summary>
        /// Disconnects the specified IServerPlayer object.
        /// </summary>
        /// <param name="player">The Player to disconnect.</param>
        public void Disconnect(IPlayer player)
        {
            Socket connection = this.playerConnections[player].CurrentSocket;
            if (connection != null && connection.Connected)
            {
                connection.Shutdown(SocketShutdown.Both);
                this.playerConnections.Remove(player);
                this.ConnectedPlayers.Remove(player);

                this.OnPlayerDisconnected(player);
            }
        }

        /// <summary>
        /// Disconnects everyone from the server.
        /// </summary>
        public void DisconnectAll()
        {
            // Loop through each connection in parallel and disconnect them.
            foreach (KeyValuePair<IPlayer, PlayerConnectionState> playerConnection in this.playerConnections.AsParallel())
            {
                // Hold a locally scoped reference to avoid parallel issues.
                Socket connection = playerConnection.Value.CurrentSocket;
                IPlayer player = playerConnection.Key;

                if (connection != null && connection.Connected)
                {
                    connection.Shutdown(SocketShutdown.Both);
                    this.OnPlayerDisconnected(player);
                }
            }

            this.playerConnections.Clear();
            this.ConnectedPlayers.Clear();
        }

        /// <summary>
        /// Called when a player connects.
        /// </summary>
        /// <param name="newPlayer">The new player.</param>
        protected virtual void OnPlayerConnected(IPlayer newPlayer)
        {
            EventHandler<ServerConnectionEventArgs> handler = this.PlayerConnected;
            if (handler == null)
            {
                return;
            }

            handler(this, new ServerConnectionEventArgs(newPlayer));
        }

        /// <summary>
        /// Called when a player disconnects.
        /// </summary>
        /// <param name="player">The player.</param>
        protected virtual void OnPlayerDisconnected(IPlayer player)
        {
            EventHandler<ServerConnectionEventArgs> handler = this.PlayerDisconnected;
            if (handler == null)
            {
                return;
            }

            handler(this, new ServerConnectionEventArgs(player));
        }

        /// <summary>
        /// Connects the client to the server and then passes the connection responsibilities to the client object.
        /// </summary>
        /// <typeparam name="TPlayer">The type of the player.</typeparam>
        /// <param name="result">The async result.</param>
        private void ConnectClient(IAsyncResult result)
        {
            // Connect and register for network related events.
            Socket connection = this.serverSocket.EndAccept(result);

            // Fetch the next incoming connection.
            this.serverSocket.BeginAccept(new AsyncCallback(this.ConnectClient), this.serverSocket);

            // Cast the invoker to IPlayer. If the Invoker is not an IPlayer, we want to throw an exception.
            IPlayer player = CharacterFactory.CreatePlayer(this.Game);

            // Initialize the player.
            Task task = player.Initialize();

            // Since we are running on a background thread to begin with, we will just wait for the 
            // player initialization operation to complete.
            task.Wait();

            lock (this.ConnectedPlayers)
            {
                this.ConnectedPlayers.Add(player);
                player.Information.Name = string.Format("Player {0}", this.ConnectedPlayers.Count);
            }

            var connectionState = new PlayerConnectionState(player, connection, UserConnectionBufferSize);
            lock (this.playerConnections)
            {
                this.playerConnections.Add(player, connectionState);
            }

            connectionState.StartListeningForData();
            this.OnPlayerConnected(player);
        }
    }
}
