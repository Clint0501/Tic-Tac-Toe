using System;

namespace Script.GameCore
{
    public interface IEventListener
    {
        void AttachEvent(EventKey eventName, Action<IEvent> action);
        void DetachAllEvent();
    }

    public interface IEvent
    {
        EventKey m_Key { get; }
    }

    public class BaseEvent : IEvent
    {
        public EventKey m_Key { get; set; }
    }

    public class PlayerChessDownEvent : IEvent
    {
        public EventKey m_Key
        {
            get => EventKey.PlayerChessDown;
        }

        public PlayerData m_PlayerData;

        public int m_GridIndex;
    }
    
    public class StartGameEvent : IEvent
    {
        public EventKey m_Key
        {
            get => EventKey.GameStart;
        }

        public GameModeEnum m_GameMode;
    }
    
    public class OpenUIEvent : IEvent
    {
        public EventKey m_Key
        {
            get => EventKey.OpenView;
        }

        public string m_ViewName;
        
        public object[] m_Args;
    }
}