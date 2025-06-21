using Script.GameCore.PlayerCtrl;

namespace Script.GameCore.Util
{
    public static class EventUtil
    {
        public static void SendChessDownEvent(PlayerData playerData,int index)
        {
            PlayerChessDownEvent evt = new PlayerChessDownEvent
            {
                m_PlayerData = playerData,
                m_GridIndex = index
            };
            EventManager.GetInstance().DispatchEvent(evt);
        }

        public static void SendGameOverEvent(PlayerData winner)
        {
            GameOverEvent evt = new GameOverEvent
            {
                m_PlayerData = winner
            };
            EventManager.GetInstance().DispatchEvent(evt);
            
        }

        public static void SendOpenViewEvent(string viewName, bool closeOtherView = false, params object[] args)
        {
            OpenUIEvent evt = new OpenUIEvent
            {
                m_ViewName = viewName,
                m_Args = new object[]{ args },
                m_ForceCloseOtherView = closeOtherView
            };
            EventManager.GetInstance().DispatchEvent(evt);
        }
        
        public static void SendCloseViewEvent(string viewName, params object[] args)
        {
            CloseUIEvent evt = new CloseUIEvent
            {
                m_ViewName = viewName
            };
            EventManager.GetInstance().DispatchEvent(evt);
        }

        public static void SendStartGameEvent(GameModeEnum gameMode)
        {
            StartGameEvent startGameEvent  = new StartGameEvent
            {
                m_GameMode = gameMode
            };
            EventManager.GetInstance().DispatchEvent(startGameEvent);
        }
    }
}