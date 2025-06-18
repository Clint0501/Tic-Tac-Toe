using System;
using System.Collections.Generic;
using Script.Common;

namespace Script.GameCore
{
    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<EventKey,List<Action<IEvent>>> m_EventKey2Actions = new Dictionary<EventKey,List<Action<IEvent>>>();
        
        
        public void AttachEvent(EventKey eventKey, Action<IEvent> action)
        {
            if (m_EventKey2Actions.TryGetValue(eventKey, out List<Action<IEvent>> actions))
            {
                actions.Add(action);
            }
            else
            {
                m_EventKey2Actions.Add(eventKey, new List<Action<IEvent>>());
                m_EventKey2Actions[eventKey].Add(action);
            }
        }

        public void DetachEvent(EventKey eventKey, Action<IEvent> action)
        {
            if (m_EventKey2Actions.TryGetValue(eventKey, out List<Action<IEvent>> actions))
            {
                actions.Remove(action);
            }
        }

        public void DispatchBaseEvent(EventKey eventKey)
        {
            BaseEvent baseEvent = new BaseEvent
            {
                m_Key = eventKey
            };
            if (m_EventKey2Actions.TryGetValue(eventKey, out List<Action<IEvent>> actions))
            {
                foreach (Action<IEvent> action in actions)
                {
                    action(baseEvent);
                }
            }
        }
        public void DispatchEvent(IEvent eventObject)
        {
            
            if (m_EventKey2Actions.TryGetValue(eventObject.m_Key, out List<Action<IEvent>> actions))
            {
                foreach (Action<IEvent> action in actions)
                {
                    action(eventObject);
                }
            }
        }
    }
}