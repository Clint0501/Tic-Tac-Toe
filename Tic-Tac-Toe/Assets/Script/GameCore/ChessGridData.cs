namespace Script.GameCore
{
    public class PlayerData
    {
        private int m_ID;

        public int ID => m_ID;

        public PlayerData(int id,int markId)
        {
            m_ID = id;
            m_MarkID = markId;
        }

        private int m_MarkID;
        
        public int MarkID => m_MarkID;
    }
    
    public class ChessGridData
    {
        
        private PlayerData m_PlayerData;
        
        public PlayerData PlayerData
        {
            set => m_PlayerData = value;
            get => m_PlayerData;
        }
    }
}