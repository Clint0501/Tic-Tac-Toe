namespace Script.GameCore.Util
{
    public static class UIUitl
    {
        public static void ShowErrorTips(string message)
        {
            OpenUIEvent evt = new OpenUIEvent
            {
                m_ViewName = "ErrorTipsView",
                m_Args = new object[]{ message }
            };
            EventManager.GetInstance().DispatchEvent(evt);
        }
    }
}