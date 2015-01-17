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

        public TestNotificationFixture(INotificationCenter notificationCenter)
        {
            this.NotificationManager = notificationCenter;
        }

        public INotificationCenter NotificationManager { get; private set; }

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

        public void Register(Action<TMessageType, ISubscription> message, Func<TMessageType, bool> condition = null)
        {
            this.callback = message;
        }

        public void Unsubscribe()
        {
            this.NotificationManager.Unsubscribe<TMessageType>(this);
        }
    }
}
