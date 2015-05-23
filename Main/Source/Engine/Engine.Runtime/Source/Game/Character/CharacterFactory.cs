//-----------------------------------------------------------------------
// <copyright file="CharacterFactory.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game.Character
{
    using System;

    /// <summary>
    /// A Factory used to create Character types.
    /// </summary>
    public static class CharacterFactory
    {
        /// <summary>
        /// The player type to instance
        /// </summary>
        private static Func<IGame, IPlayer> factoryDelegate;

        /// <summary>
        /// Creates a new player instance.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public static IPlayer CreatePlayer(IGame game)
        {
            if (factoryDelegate == null)
            {
                ICommandManager commandManager = CommandManagerFactory.CreateManager();
                return new DefaultPlayer(game, commandManager);
            }

            return factoryDelegate(game);
        }

        /// <summary>
        /// Sets the factory method used to create a new player.
        /// </summary>
        /// <param name="factory"></param>
        public static void SetFactory(Func<IGame, IPlayer> factory)
        {
            factoryDelegate = factory;
        }
    }
}
