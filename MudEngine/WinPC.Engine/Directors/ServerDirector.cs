﻿using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Linq;
using WinPC.Engine.Abstract.Directors;
using WinPC.Engine.Abstract.Networking;
using WinPC.Engine.Core;
using WinPC.Engine.States;
using WinPC.Engine.Abstract.Core;

namespace WinPC.Engine.Directors
{
    public class ServerDirector : IServerDirector
    {

        public List<Thread> ConnectionThreads { get; private set; }
        public List<Player> ConnectedPlayers { get; private set; }

        public Dictionary<Player, Thread> ConnectedUsers { get; private set; }

        public IServer Server { get; set; }

        public ServerDirector(IServer server)
        {
            Server = server;
            ConnectedPlayers = new List<Player>(Server.MaxConnections);
            ConnectionThreads = new List<Thread>(Server.MaxConnections);
            ConnectedUsers = new Dictionary<Player, Thread>();
        }

        public void AddConnection(Socket connection)
        {
            var player = new Player(new ConnectState(this), connection);
            ConnectedPlayers.Add(player);

            Thread userThread = new Thread(ReceiveDataThread);
            
            //TODO: Replace ConnectedPlayers and ConnectionThreads with ConnectedUsers
            ConnectedUsers.Add(player, userThread);

            ConnectionThreads.Add(userThread);
            var index = ConnectionThreads.Count - 1;

            ConnectionThreads[index].Name = "Player";
            ConnectionThreads[index].Start(index);


        }

        public void ReceiveDataThread(object index)
        {
            var player = ConnectedPlayers[(int)index];

            while (Server.Enabled)
            {
                player.CurrentState.Render((int)index);
                var command = player.CurrentState.GetCommand();
                command.Execute();
            }
        }

        public void DisconnectAll()
        {
            foreach (var player in ConnectedPlayers)
            {
                player.Disconnect();
            }

            foreach (var thread in ConnectionThreads)
            {
                thread.Abort();

            }

            ConnectedPlayers.Clear();
            ConnectionThreads.Clear();
        }


        public String RecieveInput(int index)
        {
            string input = String.Empty;

            while (true)
            {
                try
                {
                    byte[] buf = new byte[1];
                    Int32 recved = ConnectedPlayers[index].Connection.Receive(buf);

                    if (recved > 0)
                    {
                        if (buf[0] == '\n' && ConnectedPlayers[index].buffer.Count > 0)
                        {
                            if (ConnectedPlayers[index].buffer[ConnectedPlayers[index].buffer.Count - 1] == '\r')
                                ConnectedPlayers[index].buffer.RemoveAt(ConnectedPlayers[index].buffer.Count - 1);

                            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
                            input = enc.GetString(ConnectedPlayers[index].buffer.ToArray());
                            ConnectedPlayers[index].buffer.Clear();
                            //Return a trimmed string.
                            return input;
                        }
                        else
                            ConnectedPlayers[index].buffer.Add(buf[0]);
                    }
                    else if (recved == 0) //Disconnected
                    {
                        //   ConnectedPlayers[index]. Connected = false;
                        //  this.LoggedIn = false;
                        return "Disconnected.";
                    }
                }
                catch (Exception e)
                {
                    //Flag as disabled 
                    //  this.Connected = false;
                    //  this.LoggedIn = false;
                    return e.Message;
                }
            }
        }

        /// <summary>
        /// Returns a reference to the specified player if s/he is connected to the server.
        /// </summary>
        /// <param name="player">Name of the player to return</param>
        /// <returns></returns>
        public bool GetPlayer(string name, out IPlayer player)
        {
            var connectedPlayer = from p in ConnectedUsers
                                  where p.Key.Name == name
                                  select p.Key;

            player = connectedPlayer.First();

            return player == null ? true : false;
        }
    }
}