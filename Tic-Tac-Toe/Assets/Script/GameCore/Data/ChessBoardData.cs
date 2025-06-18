using System.Collections.Generic;
using Script.Common;

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

    public class ChessBoardData : Singleton<ChessBoardData>
    {
        #region 游戏数据

        public List<ChessGridData>  m_ChessGridDataList { get; private set; }

        public PlayerData m_PlayerData1 { get; private set; }
        
        public PlayerData m_PlayerData2 { get; private set; }
        
        #endregion
        
        public void Start()
        {
            
        }

        public bool TrySetChessGridData(PlayerData chessGridPlayerData, int index)
        {
            if (m_ChessGridDataList.Count > index && m_ChessGridDataList[index].PlayerData == null)
            {
                m_ChessGridDataList[index].PlayerData = chessGridPlayerData;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}