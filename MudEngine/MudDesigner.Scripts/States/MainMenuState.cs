﻿using System.Net.Sockets;
using System.Text;
using MudDesigner.Engine.Core;
using MudDesigner.Engine.Commands;
using MudDesigner.Engine.Core;
using MudDesigner.Engine.Directors;

namespace MudDesigner.Engine.States
{
    public class MainMenuState : IState
    {

        public ServerDirector Director { get; private set; }
        
        private Socket connection { get; set; }
        private ASCIIEncoding encoding { get; set; }
        private IPlayer Player { get; set; }

        public MainMenuState(ServerDirector director)
        {
            Director = director;
            encoding = new ASCIIEncoding();

        }
        public void Render(IPlayer connectedPlayer)
        {
            connection = connectedPlayer.Connection;
            Player = connectedPlayer;

            var player = Player as Player;
            if(player != null)
            {
                connection.Send(encoding.GetBytes(string.Format("Welcome {0}, What do you want to do? {1}",player.CharacterName,"\n\r")));    
            }
            else
            {
                connection.Send(encoding.GetBytes(string.Format("Something seriously wrong happened... What did you do!!!")));    
            }


            // Some Fancy Menu
            player.SendMessage("");
            player.SendMessage("-----------------------------------------");
            player.SendMessage(string.Format("|{0}|", Director.Server.Game.Name)); // @ToDo: I'll look into Text Centering stuff.
            player.SendMessage("-----------------------------------------");
            player.SendMessage("| [Enter] a town                        |");
            player.SendMessage("| [Join] a chat channel                 |");
            player.SendMessage("| [Save] my current player              |");
            player.SendMessage("| Change some game [Options]            |");
            player.SendMessage("| [Quit] the game                       |");
            player.SendMessage("-----------------------------------------"); 



        }

        public ICommand GetCommand()
        {
            var input = Director.RecieveInput(Player);

            // We Don't have any commands here yet... but we will! (EnterCommand, JoinCommand, SaveCommand, OptionsCommand, QuitCommand etc)
            return new InvalidCommand(connection);
        }
    }
}