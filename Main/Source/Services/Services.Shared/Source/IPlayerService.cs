using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mud.Engine.Runtime.Game.Character;
using Mud.Engine.Runtime.Services;

namespace Source
{
    public interface IPlayerService : IService
    {
        IPlayer GetPlayerByName(string name, string password);

        Guid SavePlayer(IPlayer player);

        PurgedPlayerInformation PurgeInactivePlayers();
    }
}
