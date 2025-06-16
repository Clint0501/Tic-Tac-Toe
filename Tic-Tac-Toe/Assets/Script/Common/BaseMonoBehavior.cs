using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.GameCore
{
    public class BaseMonoBehavior: MonoBehaviour,IEventListener
    {
        private Dictionary<EventKey, List<Action<object>>> m_EventKey2Actions = new Dictionary<EventKey, List<Action<object>>>();
        
        public void AttachEvent(EventKey eventName, Action<object> action)
        {
            EventManager.GetInstance().AttachEvent(eventName, action);
            if (m_EventKey2Actions.TryGetValue(eventName, out List<Action<object>> actions))
            {
                actions.Add(action);
            }
            else
            {
                m_EventKey2Actions.Add(eventName, new List<Action<object>>());
                m_EventKey2Actions[eventName].Add(action);
            }
        }

        public void DetachAllEvent()
        {
            foreach (KeyValuePair<EventKey, List<Action<object>>> pair in m_EventKey2Actions)
            {
                EventKey key = pair.Key;
                foreach (Action<object> action in pair.Value)
                {
                    EventManager.GetInstance().DeAttachEvent(key, action);
                }
                
            }
        }

        private void Awake()
        {
            OverrideAwake();
        }
        
        private void OnEnable()
        {
            OverrideEnable();
        }

        private void Start()
        {
            OverrideStart();
        }

        private void Update()
        {
            OverrideUpdate();
        }

        private void OnDestroy()
        {
            OverrideDestroy();
        }

        private void OnDisable()
        {
            DetachAllEvent();
            OverrideDisable();
        }

        protected virtual void OverrideAwake()
        {
            
        }

        protected virtual void OverrideEnable()
        {
            
        }

        protected virtual void OverrideStart()
        {
            
        }

        protected virtual void OverrideUpdate()
        {
            
        }

        protected virtual void OverrideDestroy()
        {
            
        }

        protected virtual void OverrideDisable()
        {
            
        }
    }
}