using System;

namespace Script.GameCore
{
    public interface IEventListener
    {
        void AttachEvent(EventKey eventName, Action<object> action);
        void DetachAllEvent();
    }
}