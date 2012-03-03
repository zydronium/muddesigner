﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using MudEngine.Networking;
using MudEngine.Core;
using MudEngine.Game.Characters;
using MudEngine.DAL;

namespace MudEngine.Game
{
    /// <summary>
    /// StandardGame will be the base of all Mud Games created with the engine.
    /// It manages all of the game components including the Server and the Game World.
    /// </summary>
    public class StandardGame
    {
        /// <summary>
        /// Gets or Sets the Name of this game.
        /// </summary>
        public String Name { get; set; }
        
        /// <summary>
        /// Gets or Sets the website where this game can be played at.
        /// </summary>
        public String Website { get; set; }

        /// <summary>
        /// Gets or Sets the Description of this game.  This is often displayed upon first connection.
        /// Note that this is displayed BEFORE the Servers MOTD.
        /// </summary>
        public String Description { get; set; }

        /// <summary>
        /// Gets or Sets the Version of this game.
        /// </summary>
        public String Version { get; set; }

        /// <summary>
        /// Gets or Sets if Room Names will be shown to the player each time they travel to a new Room.
        /// </summary>
        public Boolean HiddenRoomNames { get; set; }

        /// <summary>
        /// Gets or Sets if this Game is currently being played on a Server
        /// </summary>
        public Boolean Multiplayer { get; set; }

        /// <summary>
        /// Gets or Sets the minimum password size required for user account passwords
        /// </summary>
        public Int32 MinimumPasswordSize { get; set; }

        /// <summary>
        /// Get or Sets if the game will automatically save the world.  For servers with poor specifications, this can be disabled to
        /// help with performance.  Manually saving the world will be required.
        /// </summary>
        public Boolean AutoSave { get; set; }

        /// <summary>
        /// Gets if the game is currently running or not.
        /// </summary>
        public Boolean Enabled { get; private set; }

        /// <summary>
        /// Gets or Sets if the game is in debug more or not.
        /// </summary>
        public Boolean Debugging { get; set; }

        /// <summary>
        /// Gets or reference to the currently running Server.
        /// </summary>
        public Server Server { get; protected set; }

        /// <summary>
        /// Gets or Sets the paths that content is saved to.
        /// </summary>
        public DataPaths SavePaths { get; set; }

        /// <summary>
        /// StandardGame constructor.  If no Port number is provided, 4000 is used.
        /// </summary>
        /// <param name="name"></param>
        public StandardGame(String name) : this(name, 4000)
        {
        }

        /// <summary>
        /// StandardGame constructor.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="port"></param>
        public StandardGame(String name, Int32 port)
        {
            Logger.WriteLine("Initializing Standard Mud Game");
            this.Name = name;
            this.Website = "http://scionwest.net";
            this.Description = "A sample Mud game created using the Mud Designer kit.";
            this.Version = "1.0";
            this.Multiplayer = true;
            this.MinimumPasswordSize = 8;
            this.AutoSave = true;
         
            //Setup our server.
            this.Server = new Server(this, port);

            //Setup default save paths.
            DataPaths paths = new DataPaths();
            paths.Environments = @"\Environment";
            paths.Characters = @"\Characters";
            paths.Players = @"\SavedPlayer";
            paths.Scripts = @"\Scripts";

            this.SavePaths = paths;
        }

        /// <summary>
        /// Starts the game by getting all of the game scripts, loading the world
        /// loading all of the games commands and starting the server.
        /// </summary>
        /// <param name="maxPlayers"></param>
        /// <param name="maxQueueSize"></param>
        /// <returns></returns>
        public Boolean Start(Int32 maxPlayers, Int32 maxQueueSize)
        {
            Logger.WriteLine("Starting up Standard Game");
            
            //Instance Script Engine

            //Compile any scripts
            
            //Load our Commands
            CommandSystem.LoadCommands();

            //Load World

            //Start our server.
            this.Server.Start(maxPlayers, maxQueueSize);

            //If the server started without error, flag the Game as enabled.
            if (this.Server.Enabled)
                this.Enabled = true;

            return this.Enabled;
        }

        /// <summary>
        /// Stops the game but unloading the world, shutting down the server and unloading all scripts/commands.
        /// </summary>
        public void Stop()
        {
            //Save the world.

            //Stop the server
            this.Server.Stop();

            //Stop the world.

            //Purge all scripts and commands.
            CommandSystem.PurgeCommands();

            //Disable the game.
            this.Enabled = false;
        }

        public void Update()
        {

        }
    }
}
