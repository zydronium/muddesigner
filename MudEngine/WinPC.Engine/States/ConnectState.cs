﻿using System;
using System.Net.Sockets;
using System.Text;
using WinPC.Engine.Abstract.Core;
using WinPC.Engine.Commands;
using WinPC.Engine.Directors;
using WinPC.Engine.Core;

namespace WinPC.Engine.States
{
    public class ConnectState : IState
    {
        public ServerDirector Director { get; private set; }

        private Socket connection;
        private ASCIIEncoding encoding;
        private IPlayer player;

        public ConnectState(ServerDirector director)
        {
            Director = director;
            encoding = new ASCIIEncoding();

        }
        public void Render(IPlayer connectedPlayer)
        {
            connection = connectedPlayer.Connection;
            player = connectedPlayer;

            connection.Send(encoding.GetBytes("Welcome to Scionwest's Mud Engine!" + "\n\r"));
            connection.Send(encoding.GetBytes("Please enter your name" + "\n\r"));
        }

        public ICommand GetCommand()
        {

            var input = Director.RecieveInput(player);

            if (input == "menu")
            {
                return new SwitchStateCommand(Director, new MainMenuState(Director), player);
            }

            return new InvalidCommand(connection);
        }
    }
}