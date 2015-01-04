//-----------------------------------------------------------------------
// <copyright file="ITravelDirection.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game.Environment
{
    public interface ITravelDirection
    {
        string Direction { get; }

        ITravelDirection GetOppositeDirection();
    }
}