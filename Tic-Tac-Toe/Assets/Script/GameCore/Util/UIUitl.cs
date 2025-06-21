namespace Script.GameCore.Util
{
    public static class UIUitl
    {
        public static void ShowErrorTips(string message)
        {
            EventUtil.SendOpenViewEvent("ErrorTipsView", false, message);
        }
        
        
    }
}