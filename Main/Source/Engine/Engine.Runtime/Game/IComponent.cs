using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game
{
    public interface IComponent
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        int Id { get; set; }
    }
}
