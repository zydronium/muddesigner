//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Apps.Windows.Desktop.Server.App
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Autofac;
    using Mud.Engine.Runtime.Networking;
    using Mud.Engine.Runtime.Game;
    using Mud.Engine.Components.WindowsServer;
    using Mud.Engine.Runtime.Game.Character;
    using Mud.Engine.Runtime;
    using Mud.Engine.Runtime.Game.Environment;
    using Mud.Data.FlatFile;
    using Mud.Data.Shared;
    using Mud.Apps.Desktop.Windows.ServerApp;
    using Mud.Engine.Runtime.Services;
    using System.Threading.Tasks;


    /// <summary>
    /// The Mud Designer Telnet Server.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The Dependency Injection container
        /// </summary>
        private static IContainer container;

        /// <summary>
        /// The engine game server
        /// </summary>
        private static IServer server;

        /// <summary>
        /// The game
        /// </summary>
        private static DefaultGame game;

        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            RegisterContainerTypes();

            // Instance a new DesktopGame and try to initialize it.
            game = container.Resolve<DefaultGame>();

            try
            {
                Task initializationTask = game.Initialize();
                initializationTask.Wait();
            }
            catch (Exception)
            {
                // Temporary
                throw;
            }

            // Instance our Default Server. This server is for Windows Desktop only.
            server = new DefaultServer();
            server.Port = 23;
            server.MaxConnections = 100;

            // Register to be notified when a player connects and disconnects.
            server.PlayerConnected += Server_PlayerConnected;
            server.PlayerDisconnected += Server_PlayerDisconnected;

            // Start the server. The DefaultPlayer Type will be instanced when each new player connects.
            // TODO: 11/2/14 - Add a non-generic Start method accepting a Type for IoC support.
            server.Start<DefaultPlayer>(game);
            game.IsMultiplayer = true;

            SetupGameWorld(game);

            // Our game loop.
            while (server.Status == ServerStatus.Running)
            {
                Thread.Sleep(500);
            }

            // Check if the server has not stopped. If not, we stop.
            if (server.Status != ServerStatus.Stopped)
            {
                Task<bool> requestResult = game.RequestShutdown();
                requestResult.Wait();

                if (requestResult.Result)
                {
                    server.Stop();
                }
            }
        }

        /// <summary>
        /// Handles the PlayerDisconnected event of the server control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ServerConnectionEventArgs"/> instance containing the event data.</param>
        private static void Server_PlayerDisconnected(object sender, ServerConnectionEventArgs e)
        {
            e.Player.MessageSent -= Player_MessageSent;
        }

        /// <summary>
        /// Handles the PlayerConnected event of the server control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="ServerConnectionEventArgs"/> instance containing the event data.</param>
        private static void Server_PlayerConnected(object sender, ServerConnectionEventArgs e)
        {
            e.Player.MessageSent += Player_MessageSent;
        }

        /// <summary>
        /// Player_s the message sent.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private static void Player_MessageSent(object sender, InputArgs e)
        {
        }

        /// <summary>
        /// Registers the container types.
        /// </summary>
        private static void RegisterContainerTypes()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<LoggingService>().As<ILoggingService>();
            builder.RegisterType<FileStorageService>().As<IFileStorageService>();

            // Engine runtime types
            builder.RegisterType<DefaultGame>().As<DefaultGame>();
            builder.RegisterType<WorldService>().As<IWorldService>();

            // Flat File data store types
            builder.RegisterType<WorldRepository>().As<IWorldRepository>();
            builder.RegisterType<TimeOfDayStateRepository>().As<ITimeOfDayStateRepository>();
            container = builder.Build();
        }

        /// <summary>
        /// Sets up the game world.
        /// </summary>
        /// <param name="game">The game.</param>
        private static void SetupGameWorld(DefaultGame game)
        {
            // Set up the Zone            
            var weatherStates = new List<IWeatherState> { new ClearWeather(), new RainyWeather(), new ThunderstormWeather() };
            DefaultZone zone = new DefaultZone();
            zone.Name = "Country Side";
            zone.WeatherStates = weatherStates;
            zone.WeatherUpdateFrequency = 6;
            zone.WeatherChanged += (sender, weatherArgs) => Console.WriteLine(string.Format("{0} zone weather has changed to {1}", zone.Name, weatherArgs.CurrentState.Name));
            DefaultZone zone2 = new DefaultZone();
            zone2.Name = "Castle Rock";
            zone2.WeatherStates = weatherStates;
            zone2.WeatherUpdateFrequency = 2;
            zone2.WeatherChanged += (sender, weatherArgs) => Console.WriteLine(string.Format("{0} zone weather has changed to {1}", zone2.Name, weatherArgs.CurrentState.Name));

            // Set up the World.
            DefaultWorld world = new DefaultWorld();
            world.GameDayToRealHourRatio = 0.2;
            world.HoursPerDay = 10;
            world.Name = "Sample World";

            var morningState = new TimeOfDayState { Name = "Morning", StateStartTime = new TimeOfDay { Hour = 2 } };
            var afternoonState = new TimeOfDayState { Name = "Afternoon", StateStartTime = new TimeOfDay { Hour = 5 } };
            var nightState = new TimeOfDayState { Name = "Night", StateStartTime = new TimeOfDay { Hour = 8 } };

            world.TimeOfDayStates = new List<TimeOfDayState> { morningState, afternoonState, nightState };
            world.TimeOfDayChanged += World_TimeOfDayChanged;

            // Set up the Realm.
            DefaultRealm realm = new DefaultRealm();
            realm.TimeZoneOffset = new TimeOfDay { Hour = 3, Minute = 10, HoursPerDay = world.HoursPerDay };
            realm.Name = "Realm 1";

            // Initialize the environment.
            world.Initialize();
            world.AddRealmToWorld(realm);
            realm.AddZoneToRealm(zone);
            realm.AddZoneToRealm(zone2);
            game.Worlds.Add(world);
        }

        /// <summary>
        /// Handles the TimeOfDayChanged event of the world control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TimeOfDayChangedEventArgs"/> instance containing the event data.</param>
        private static void World_TimeOfDayChanged(object sender, TimeOfDayChangedEventArgs e)
        {
            // If we have a previous time of day, unregister our event.
            if (e.TransitioningFrom != null)
            {
                e.TransitioningFrom.TimeUpdated -= CurrentTimeOfDay_TimeUpdated;
            }

            e.TransitioningTo.TimeUpdated += CurrentTimeOfDay_TimeUpdated;
            CurrentTimeOfDay_TimeUpdated(sender, e.TransitioningTo.CurrentTime);
        }

        /// <summary>
        /// Handles the TimeChanged event from within a TimeOfDayState.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The current time of day.</param>
        private static void CurrentTimeOfDay_TimeUpdated(object sender, TimeOfDay e)
        {
            if (!(sender is TimeOfDayState))
            {
                return;
            }

            // Indicates a new hour has passed.
            string hour = string.Empty;
            string minute = string.Empty;

            if (e.Hour < 10)
            {
                hour = string.Format("0{0}", e.Hour);
            }
            else
            {
                hour = e.Hour.ToString();
            }

            if (e.Minute < 10)
            {
                minute = string.Format("0{0}", e.Minute);
            }
            else
            {
                minute = e.Minute.ToString();
            }

            string timeOfDay = string.Empty;
            if (e.Hour < 12)
            {
                timeOfDay = "AM";
            }
            else
            {
                timeOfDay = "PM";
            }

            TimeOfDayState timeOfDayState = (TimeOfDayState)sender;

            Console.WriteLine(string.Format("World time is {0}:{1} {2} in the {3}", hour, minute, timeOfDay, timeOfDayState.Name));
            foreach (DefaultRealm realm in game.Worlds.FirstOrDefault().Realms)
            {
                Console.WriteLine(string.Format("{0} world time is {1} in the {2}", realm.Name, realm.CurrentTimeOfDay.ToString(), realm.GetCurrentTimeOfDayState().Name));
            }

            Console.WriteLine(Environment.NewLine);
        }
    }
}
