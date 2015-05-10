//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Apps.Windows.Desktop.Server.App
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Autofac;
    using Mud.Data.Shared;
    using Mud.Engine.Components.WindowsServer;
    using Mud.Engine.Runtime.Game;
    using Mud.Engine.Runtime.Game.Character;
    using Mud.Engine.Runtime.Networking;
    using Mud.Engine.Runtime.Services;

    /// <summary>
    /// The Mud Designer Telnet Server.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            string bootstrapTypeName = string.Empty;
            if (args.Any(arg => arg.ToLower().StartsWith("bootstraptype")))
            {
                bootstrapTypeName = args.First(arg => arg.ToLower().StartsWith("bootstraptype="));
                bootstrapTypeName = bootstrapTypeName.Substring("bootstraptype=".Length);
            }
            else
            {
                bootstrapTypeName = typeof(DefaultServerBootstrap).FullName;
            }

            Type bootstrapType = Type.GetType(bootstrapTypeName);
            var bootstrap = (ServerBootstrap)Activator.CreateInstance(bootstrapType);
            Task bootstrapTask = bootstrap.Initialize();
            bootstrapTask.Wait();
        }
    }
}
