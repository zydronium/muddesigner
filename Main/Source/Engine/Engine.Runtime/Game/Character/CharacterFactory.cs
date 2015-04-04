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
        private static Func<IGame, IPlayer> playerFactory;

        /// <summary>
        /// Creates a new player instance.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public static IPlayer CreatePlayer(IGame game)
        {
            if (playerFactory == null)
            {
                return new DefaultPlayer(game);
            }

            return playerFactory(game);
        }

        /// <summary>
        /// Sets the factory method used to create a new player.
        /// </summary>
        /// <param name="factoryCallback"></param>
        public static void SetFactory(Func<IGame, IPlayer> factoryCallback)
        {
            playerFactory = factoryCallback;
        }
    }
}
