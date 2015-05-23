using Mud.Engine.Runtime.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Engine.Runtime.Fixtures
{
    public class TestNotificationFixture<TMessageType> : INotification<TMessageType> where TMessageType : class, IMessage
    {
        private Action<TMessageType, ISubscription> callback;

        private Func<TMessageType, bool> condition;

        public bool IsActive { get; private set; }

        public event Action<NotificationArgs> Unsubscribing;

        public void ProcessMessage(TMessageType message)
        {
            if (condition == null)
            {
                callback(message, this);
            }
            else if (condition(message))
            {
                callback(message, this);
            }
        }

        public void Register(Action<TMessageType, ISubscription> callback, Func<TMessageType, bool> condition = null)
        {
            this.callback = callback;
            this.condition = condition;
        }

        public void Unsubscribe()
        {
            var handler = this.Unsubscribing;
            if (handler != null)
            {
                handler(new NotificationArgs(this, typeof(TMessageType)));
            }
        }
    }
}
