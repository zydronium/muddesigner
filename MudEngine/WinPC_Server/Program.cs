﻿using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Xml.Linq;

using MudEngine.Game;
using MudEngine.Game.Characters;
using MudEngine.GameScripts;
using MudEngine.Core;
using MudEngine.DAL;

namespace WinPC_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //Setup the engines log system
            Logger.LogFilename = "StandardGame.Log";
            Logger.Enabled = true;
            Logger.ConsoleOutPut = true;

            //Instance and start the game.  This will start the server.
            StandardGame game = new StandardGame("Sample Game");
            game.Start();

            StandardCharacter character = new StandardCharacter(game, "Player 1", "Connected player to the server");
            character.Password = "1233456";
            character.Immovable = true;
            character.Save("Character1.txt");

            //Setup our Server console input class
            ConsoleInput input = new ConsoleInput();

            //Run the console input on its own thread.
            Thread inputThread = new Thread(input.GetInput);
            inputThread.Start();

            //Game loops until it is disabled.
            while (game.Enabled)
            {
                //Check the queued Console Input
                if (input.Message.Equals("exit"))
                {
                    //If the server console has a exit command entered.
                    //stop the game.  This will set game.Enabled to false.
                    game.Stop();
                }
                else
                    input.Message = String.Empty;
            }

            //Kill the Console Input thread.
            inputThread.Abort();
        }
    }
}