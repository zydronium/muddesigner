using System;

namespace Mud.Engine.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class ExceptionFactoryResult
    {
        /// <summary>
        /// Callback on the results of an ExceptionFactory invocation
        /// </summary>
        /// <param name="callback">The callback.</param>
        public void ElseDo(Action callback)
        {
            callback();
        }
    }
}
