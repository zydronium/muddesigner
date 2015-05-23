using Mud.Engine.Runtime.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mud.Engine.Runtime.Game;
using Mud.Engine.Runtime.Game.Environment;
using Mud.Engine.Runtime;
using System.Reflection;
using Autofac;
using Mud.Engine.Runtime.Game.Character;
using Mud.Engine.Components.WindowsServer;
using Mud.Engine.Runtime.Services;
using Mud.Data.Shared;

namespace Mud.Apps.Windows.Desktop.Server.App
{
    /// <summary>
    /// Sets up an IGame instance for the server to use.
    /// </summary>
    public class ServerConfiguration : IServerConfiguration
    {
        private List<string> assemblies = new List<string>();

        private IGame game;

        private ISecurityRoleRepository securityRoleRepository;

        public ServerConfiguration(ISecurityRoleRepository roleRepository)
        {
            this.securityRoleRepository = roleRepository;
        }

        /// <summary>
        /// Configures the specified game for running on the server.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <param name="server">The server.</param>
        public Task Configure(IGame game, IServer server)
        {
            server.Port = 5000;
            server.MaxConnections = 100;
            server.MessageOfTheDay = new List<string> { "Welcome to the Mud Designer Test Server!" };
            server.Owner = "Johnathon Sullinger";

            this.game = game;
            game.Information.Name = "Sample Mud Designer Game";

            return this.ConstructWorld(game);
        }

        public Task<IEnumerable<ISecurityRole>> InitializeSecurityRoles()
        {
            return this.securityRoleRepository.GetSecurityRoles();
        }

        /// <summary>
        /// Constructs the world.
        /// </summary>
        /// <param name="game">The game.</param>
        private async Task ConstructWorld(IGame game)
        {
            // Set up the Room
            var bedroom = new DefaultRoom { IsEnabled = true };
            var hallway = new DefaultRoom { IsEnabled = true };

            // Set up the Zone            
            var weatherStates = new List<IWeatherState> { new ClearWeather(), new RainyWeather(), new ThunderstormWeather() };
            DefaultZone zone = new DefaultZone();
            zone.Name = "Country Side";
            zone.Rooms = new List<DefaultRoom>() { bedroom };
            zone.WeatherStates = weatherStates;
            zone.WeatherUpdateFrequency = 6;
            zone.WeatherChanged += (sender, weatherArgs) => Console.WriteLine($"{zone.Name} zone weather has changed to {weatherArgs.CurrentState.Name}");
            DefaultZone zone2 = new DefaultZone();
            zone2.Name = "Castle Rock";
            zone2.Rooms = new List<DefaultRoom> { hallway };
            zone2.WeatherStates = weatherStates;
            zone2.WeatherUpdateFrequency = 2;
            zone2.WeatherChanged += (sender, weatherArgs) => Console.WriteLine($"{zone2.Name} zone weather has changed to {weatherArgs.CurrentState.Name}");

            // Set up the World.
            IWorld world = new DefaultWorld();
            world.GameDayToRealHourRatio = 0.4;
            world.HoursPerDay = 10;
            world.Name = "Sample World";

            var morningState = new TimeOfDayState { Name = "Morning", StateStartTime = new TimeOfDay { Hour = 2 } };
            var afternoonState = new TimeOfDayState { Name = "Afternoon", StateStartTime = new TimeOfDay { Hour = 5 } };
            var nightState = new TimeOfDayState { Name = "Night", StateStartTime = new TimeOfDay { Hour = 8 } };

            world.AddTimeOfDayState(morningState);
            world.AddTimeOfDayState(afternoonState);
            world.AddTimeOfDayState(nightState);
            world.TimeOfDayChanged += World_TimeOfDayChanged;

            // Set up the Realm.
            DefaultRealm realm = new DefaultRealm(world, new TimeOfDay { Hour = 3, HoursPerDay = 10 });
            realm.TimeZoneOffset = new TimeOfDay { Hour = 3, Minute = 10, HoursPerDay = world.HoursPerDay };
            realm.Name = "Realm 1";

            // Initialize the environment.
            Task task = world.Initialize();
            task.Wait();

            await world.AddRealmToWorld(realm);
            realm.AddZoneToRealm(zone);
            realm.AddZoneToRealm(zone2);
            await game.AddWorld(world);
        }

        /// <summary>
        /// Handles the TimeOfDayChanged event of the world control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="TimeOfDayChangedEventArgs"/> instance containing the event data.</param>
        private void World_TimeOfDayChanged(object sender, TimeOfDayChangedEventArgs e)
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
        private void CurrentTimeOfDay_TimeUpdated(object sender, TimeOfDay e)
        {
            ExceptionFactory.ThrowIf<InvalidCastException>
                (!(sender is TimeOfDayState),
                "The sender provided on TimeOfDay Changed event was not a TimeOfDayState object.");
            var timeOfDayState = (TimeOfDayState)sender;

            // Indicates a new hour has passed.
            string hour = e.Hour < 10 ? $"0{e.Hour}" : e.Hour.ToString();
            string minute = e.Minute < 10 ? $"0{e.Minute}" : e.Minute.ToString();
            string timeOfDay = e.Hour < 12 ? "AM" : "PM";

            Console.WriteLine($"World time is {hour}:{minute} {timeOfDay} in the {timeOfDayState.Name}");
            if (this.game.Worlds.Any())
            {
                foreach (DefaultRealm realm in this.game.Worlds.FirstOrDefault().Realms)
                {
                    Console.WriteLine($"{realm.Name} world time is {realm.CurrentTimeOfDay.ToString()} in the {realm.GetCurrentTimeOfDayState().Name}");
                }
            }

            Console.WriteLine(Environment.NewLine);
        }
    }
}
