namespace Script.GameCore.PlayerCtrl
{
    [Player(GameModeEnum.EasyAIPlayer)]
    public class EasyAIPlayer : AIPlayer
    {
        
        public EasyAIPlayer()
        {
            m_PlayerData = new PlayerData("简单人机");
        }
        protected override int GetPressIndex()
        {
            return GetEmptyIndex();
        }

        protected int GetEmptyIndex()
        {
            foreach (int index in ChessBoardData.GetInstance().m_ChessGridDataDic.Keys)
            {
                if (!ChessBoardData.GetInstance().IsGridOccupied(index))
                {
                    return index;
                }
            }

            return -1;
        }

        
    }
}