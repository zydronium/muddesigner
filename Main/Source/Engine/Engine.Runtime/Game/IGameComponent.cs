using System;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game
{
    public interface IGameComponent
    {
        event Func<IGameComponent, Task> Loading;

        event EventHandler<EventArgs> Loaded;

        event Func<IGameComponent, Task> Deleting;

        event EventHandler<EventArgs> Deleted;

        Task Initialize();

        Task Delete();

        Task Load();

        Task Unload();
    }
}
