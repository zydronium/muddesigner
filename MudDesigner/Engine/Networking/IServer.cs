﻿using System;
using MudDesigner.Engine.Core;

namespace MudDesigner.Engine.Networking
{
    public enum ServerStatus
    {
        Stopped = 0,
        Starting = 1,
        Running = 2
    }


    public interface IServer
    {
        int Port { get; }
        int MaxConnections { get; set; }
        int MaxQueuedConnections { get; }
        int MinimumPasswordSize { get; set; }
        bool Enabled { get; }
        ServerStatus Status { get; }

        string MOTD { get; }
        string ServerOwner { get; }

        IGame Game { get; }

        void Start(Int32 maxConnections, Int32 maxQueueSize, IGame game);
        void Stop();
        void Running();
 
    }
}