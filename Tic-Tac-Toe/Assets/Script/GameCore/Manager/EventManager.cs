using System;
using System.Collections.Generic;
using Script.Common;

namespace Script.GameCore
{
    public class EventManager : Singleton<EventManager>
    {
        private Dictionary<EventKey,List<Action<object>>> m_EventKey2Actions = new Dictionary<EventKey,List<Action<object>>>();


        public void AttachEvent(EventKey eventKey, Action<object> action)
        {
            if (m_EventKey2Actions.TryGetValue(eventKey, out List<Action<object>> actions))
            {
                actions.Add(action);
            }
            else
            {
                m_EventKey2Actions.Add(eventKey, new List<Action<object>>());
                m_EventKey2Actions[eventKey].Add(action);
            }
        }

        public void DeAttachEvent(EventKey eventKey, Action<object> action)
        {
            if (m_EventKey2Actions.TryGetValue(eventKey, out List<Action<object>> actions))
            {
                actions.Remove(action);
            }
        }

        public void DispatchEvent(EventKey eventKey,params object[] args)
        {
            if (m_EventKey2Actions.TryGetValue(eventKey, out List<Action<object>> actions))
            {
                foreach (Action<object> action in actions)
                {
                    action(args);
                }
            }
        }
    }
}