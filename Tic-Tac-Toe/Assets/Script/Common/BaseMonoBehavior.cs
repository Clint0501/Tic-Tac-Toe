using System;
using System.Collections.Generic;
using UnityEngine;

namespace Script.GameCore
{
    public class BaseMonoBehavior: MonoBehaviour,IEventListener
    {
        private Dictionary<EventKey, List<Action<IEvent>>> m_EventKey2Actions = new Dictionary<EventKey, List<Action<IEvent>>>();
        
        public void AttachEvent(EventKey eventName, Action<IEvent> action)
        {
            EventManager.GetInstance().AttachEvent(eventName, action);
            if (m_EventKey2Actions.TryGetValue(eventName, out List<Action<IEvent>> actions))
            {
                actions.Add(action);
            }
            else
            {
                m_EventKey2Actions.Add(eventName, new List<Action<IEvent>>());
                m_EventKey2Actions[eventName].Add(action);
            }
        }

        public void DetachAllEvent()
        {
            foreach (KeyValuePair<EventKey, List<Action<IEvent>>> pair in m_EventKey2Actions)
            {
                EventKey key = pair.Key;
                foreach (Action<IEvent> action in pair.Value)
                {
                    EventManager.GetInstance().DetachEvent(key, action);
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

        public virtual void InitViewData(object[] args)
        {
            
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