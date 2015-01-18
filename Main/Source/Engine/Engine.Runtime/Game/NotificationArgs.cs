using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mud.Engine.Runtime.Game
{
    public class NotificationArgs
    {
        public NotificationArgs(ISubscription subscription, Type messageType)
        {
            this.Subscription = subscription;
            this.MessageType = messageType;
        }

        public ISubscription Subscription { get; private set; }

        public Type MessageType { get; private set; }
    }
}
