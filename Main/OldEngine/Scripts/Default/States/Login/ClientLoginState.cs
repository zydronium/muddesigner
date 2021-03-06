﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using MudDesigner.Engine.Commands;
using MudDesigner.Engine.Core;
using MudDesigner.Engine.Directors;
using MudDesigner.Engine.Mobs;
using MudDesigner.Engine.Properties;
using MudDesigner.Engine.States;
using MudDesigner.Engine.Environment;
using MudDesigner.Scripts.Default.Commands;
using MudDesigner.Scripts.Default.States.CreateCharacter;
using log4net;

namespace MudDesigner.Scripts.Default.States.Login
{
    public class ClientLoginState : IState
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ClientLoginState)); 

    private ServerDirector director;
    private IPlayer connectedPlayer;

    //Used to manage the state of the Login in a more readable manor
    private enum CurrentState
    {
        EnteringName,
        EnteringPassword,
        CharacterSelection,
    }
    private CurrentState currentState;

    public ClientLoginState(ServerDirector serverDirector)
    {
        director = serverDirector;
        currentState = CurrentState.EnteringName;
    }

    public void Render(IPlayer player)
    {
        //Store a reference for the GetCommand method to use
        connectedPlayer = player;

        //Check which state we are in
        switch(currentState)
        {
                //User is entering his/her character name
            case CurrentState.EnteringName:
                player.SendMessage("What is your username, adventurer? ", false);
                break;
                //User is entering his/her password
            case CurrentState.EnteringPassword:
                player.SendMessage("Please enter your password " + player.Name + ": ", false);
                break;
        }
    }

    public ICommand GetCommand()
    {
        //Check which state we are in
        switch (currentState)
        {
                //User is entering his/her character name
            case CurrentState.EnteringName:
                {
                    GetUsername();
                    break;
                }
                //User is entering his/her password
            case CurrentState.EnteringPassword:
                {
                    IState startState = connectedPlayer.CurrentState;

                        if(GetUserPassword())
                    {
                        //Was originally LookCommand() but that was overriding the State specified in GetUserPassword.
                            return new NoOpCommand(); 
                    }
                    break;
                }
        }

        return new NoOpCommand();
    }

    private void GetUsername()
    {
        //Recieve the user input
        var input = connectedPlayer.ReceiveInput();

        //First input received on connection, so clean it.
        //Some telnet clients send header information with it.
        input = System.Text.RegularExpressions.Regex.Match(input, @"\w+").ToString();

        //Make sure the text entered is valid and not null, blank etc.
        if (!ValidateInput(input))
        {
            connectedPlayer.SendMessage("You have entered an invalid name, please try again!");
            return;
        }

        //Check if the character exists. If so, change the state of Login so s/he can enter the password
        if (UserExists(input))
        {
            connectedPlayer.Name = input;
            currentState = CurrentState.EnteringPassword;
        }
        else //No character by the supplied name found, so lets create a new one!
        {
            //New user messages are sent from within the NewCharacter state.
            connectedPlayer.Name = input;
            connectedPlayer.SwitchState(new CreationManager(director, CreationManager.CreationState.CharacterCreation));
        }
    }

    private bool GetUserPassword()
    {
        //Recieve the user input
        var input = connectedPlayer.ReceiveInput();

        //Make sure the text entered is valid and not null, blank etc.
        if (!ValidateInput(input))
        {
            connectedPlayer.SendMessage("Your password is invalid!");
            return false;
        }

        var file = new FileIO();

        IPlayer loadedplayer = (IPlayer)file.Load(
            Path.Combine(
                EngineSettings.Default.PlayerSavePath,
                string.Format("{0}.char", connectedPlayer.Username)), 
                connectedPlayer.GetType());

        if (loadedplayer != null && loadedplayer.CheckPassword(input))
        {
            /*Make sure we are disconnecting the user if they are connected already.
            foreach (var connectedUser in director.ConnectedPlayers.Keys)
            {
                if (connectedUser.Name == loadedplayer.Name && connectedUser != loadedplayer)
                    connectedUser.Disconnect();
            }
            */

            connectedPlayer.SendMessage("Success!!");

            //Can use inherited built-in CopyState method instead
            //connectedPlayer.LoadPlayer(loadedplayer);
                
            //Use IGameObject.CopyState to use a uniform method across the engine
            //A little slower than the LoadPlayer method, but it can be revised to be quicker.
            //Notes on revising the method are under GameObject.cs
            IGameObject tmp = (IGameObject)loadedplayer;
            connectedPlayer.CopyState(ref tmp); //Copies loadedPlayer state to connectedPlayer.
                
            //Make sure the player is properly added to the world.
            IWorld world = connectedPlayer.Director.Server.Game.World;
            IRealm realm = world.GetRealm(connectedPlayer.Location.Zone.Realm.Name);

            if (realm == null)
                return false;

            IZone zone = realm.GetZone(connectedPlayer.Location.Zone.Name);
            if (zone == null)
                return false;

            IRoom room = zone.GetRoom(connectedPlayer.Location.Name);
            if (room == null)
                return false;

            connectedPlayer.Move(room);

            Log.Info(string.Format("{0} has just logged in.", connectedPlayer.Name));
            connectedPlayer.SwitchState(new LoginCompleted());

            return true;

        }
        else
        {
            Log.Info(string.Format("{0} has failed logged in at IP Address: {1}.", connectedPlayer.Name,
                                    connectedPlayer.Connection.RemoteEndPoint));

            return false; 
        }
    }

       
        private bool ValidateInput(string userName)
        {
            //This is in its own method so we can add additional checks
            //in the future if we want that check for valid characters etc.
            return !string.IsNullOrEmpty(userName);
        }

        private bool UserExists(string username)
        {
            //If the path specified within the Engine Settings doesn't exist, create it.
            if (!Directory.Exists(EngineSettings.Default.PlayerSavePath))
                Directory.CreateDirectory(EngineSettings.Default.PlayerSavePath);

            //Setup our path to save, including filename
            string path = Path.Combine(EngineSettings.Default.PlayerSavePath, string.Format("{0}.char", username));

            connectedPlayer.Username = username;
            //Return true if the file exists.
            if (File.Exists(path))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
