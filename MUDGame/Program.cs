﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MUDGame.Environments;

namespace MUDGame
{
    class Program
    {
        //Setup our Fields
        static MudEngine.GameManagement.Game game;
        static MudEngine.GameManagement.CommandEngine commands;
        static MudEngine.GameObjects.Characters.Controlled.PlayerBasic player;

        static List<MudEngine.GameObjects.Environment.Realm> realmCollection;

        static void Main(string[] args)
        {
            //Initialize them
            game = new MudEngine.GameManagement.Game();
            commands = new MudEngine.GameManagement.CommandEngine();
            realmCollection = new List<MudEngine.GameObjects.Environment.Realm>();

            //Setup the game
            game.AutoSave = true;
            game.BaseCurrencyName = "Copper";
            game.BaseCurrencyAmount = 1;
            game.CompanyName = "Mud Designer";
            game.DayLength = 60 * 8;
            game.GameTitle = "Test Mud Project";
            game.HideRoomNames = false;
            game.PreCacheObjects = true;
            game.ProjectPath = MudEngine.FileSystem.FileManager.GetDataPath(MudEngine.FileSystem.SaveDataTypes.Root);
            game.TimeOfDay = MudEngine.GameManagement.Game.TimeOfDayOptions.Transition;
            game.TimeOfDayTransition = 30;
            game.Version = "0.1";
            game.Website = "http://MudDesigner.Codeplex.com";

            //Create the world
            BuildRealms();

            //Setup our starting location
            foreach (MudEngine.GameObjects.Environment.Realm realm in realmCollection)
            {
                if (realm.IsInitialRealm)
                {
                    game.SetInitialRealm(realm);
                    break;
                }
            }
            if (game.InitialRealm == null)
                Console.WriteLine("Critical Error: No Initial Realm defined!");

            //Start the game.
            MudEngine.GameManagement.CommandEngine.LoadAllCommands();
            game.IsRunning = true;

            while (game.IsRunning)
            {
                Console.Write("Command: ");
                string command = Console.ReadLine();

                MudEngine.GameManagement.CommandEngine.ExecuteCommand(command, player, game, null);
            }

            Console.WriteLine("Press Enter to exit.");
            Console.ReadKey();
        }

        static private void BuildRealms()
        {
            Zeroth zeroth = new Zeroth();
            realmCollection.Add(zeroth.BuildZeroth());
        }
    }
}
