//-----------------------------------------------------------------------
// <copyright file="IWeatherState.cs" company="Sully">
//     Copyright (c) Johnathon Sullinger. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Mud.Engine.Runtime.Game.Environment
{
    public interface IWeatherState
    {
        double OccurrenceProbability { get; }

        string Name { get; }
    }
}