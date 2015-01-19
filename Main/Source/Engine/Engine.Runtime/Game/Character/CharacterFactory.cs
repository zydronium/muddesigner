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
        // TODO: This factory needs to be a generic factory, used to fetch instances
        // based on T. ICharacter, IPlayer, INPC, IMonster etc.

        /// <summary>
        /// The player type to instance
        /// </summary>
        private static Type playerType;

        /// <summary>
        /// Initializes the factory to return the specified Character Type.
        /// </summary>
        /// <typeparam name="TPlayer"></typeparam>
        public static void Initialize<TPlayer>() where TPlayer : IPlayer
        {
            playerType = typeof(TPlayer);
        }

        /// <summary>
        /// Creates a new player instance.
        /// </summary>
        /// <param name="game">The game.</param>
        /// <returns></returns>
        public static IPlayer CreatePlayer(DefaultGame game)
        {
            // TODO: Build out a better factory not so tightly coupled to DefaultPlayer.
            return Activator.CreateInstance(playerType, new object[] { game }) as IPlayer;
        }
    }
}
