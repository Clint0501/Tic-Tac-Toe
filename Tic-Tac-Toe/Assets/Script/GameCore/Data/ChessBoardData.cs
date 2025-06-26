using System.Collections.Generic;
using Script.Common;
using Script.GameCore.PlayerCtrl;

namespace Script.GameCore
{
    public class PlayerData
    {
        string m_ID;

        public string ID => m_ID;

        public PlayerData(string id)
        {
            m_ID = id;
        }
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

        public Dictionary<int, ChessGridData>  m_ChessGridDataDic { get; private set; }

        public BasePlayer m_SelfPlayer { get; private set; }
        
        public BasePlayer m_OppoPlayer { get; private set; }

        private BasePlayer m_CurrentPlayer;
        
        private GameModeEnum m_CurrentGameMode;
        
        #endregion
        
        /// <summary>
        /// 初始化对局数据
        /// </summary>
        public void Init(GameModeEnum  mode)
        {
            m_ChessGridDataDic ??= new Dictionary<int, ChessGridData>();
            m_ChessGridDataDic.Clear();
            for (int i = 0; i < 9; i++)
            {
                m_ChessGridDataDic.Add(i, new ChessGridData());
            }

            m_SelfPlayer = new BasePlayer
            {
                m_PlayerData = new PlayerData("玩家1")
            };
            m_CurrentGameMode = mode;
            if (m_CurrentGameMode == GameModeEnum.TwoPlayer)
            {
                m_OppoPlayer = new BasePlayer
                {
                    m_PlayerData = new PlayerData("玩家2")
                };
            }
            else
            {
                m_OppoPlayer = AIPlayerFactory.GetPlayer(m_CurrentGameMode);
            }
            
            m_CurrentPlayer = m_SelfPlayer;
        }

        /// <summary>
        /// 继续游戏
        /// </summary>
        public void Continue()
        {
            Init(m_CurrentGameMode);
        }

        /// <summary>
        /// 获取当前回合的玩家
        /// </summary>
        /// <returns></returns>
        public BasePlayer GetCurrentPlayer()
        {
            return m_CurrentPlayer;
        }
        
        /// <summary>
        /// 切换玩家
        /// </summary>
        public void SwitchPlayer()
        {
            if (m_CurrentPlayer == m_SelfPlayer)
            {
                m_CurrentPlayer = m_OppoPlayer;
                return;
            }
            m_CurrentPlayer = m_SelfPlayer;
        }
        
        /// <summary>
        /// 设置棋盘格信息
        /// </summary>
        /// <param name="chessGridPlayerData"></param>
        /// <param name="index"></param>
        public void SetChessGridData(PlayerData chessGridPlayerData, int index)
        {
            m_ChessGridDataDic[index].PlayerData = chessGridPlayerData;
        }
        
        /// <summary>
        /// 获取棋盘格信息
        /// </summary>
        /// <param name="chessGridPlayerData"></param>
        /// <param name="index"></param>
        public PlayerData GetChessGridData(int index)
        {
            return m_ChessGridDataDic[index].PlayerData;
        }

        /// <summary>
        /// 获取玩家所占的棋盘格索引
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public List<int> GetPlayerOccupiedGridIndex(BasePlayer player)
        {
            List<int> occupiedGridIndex = new List<int>();
            foreach (KeyValuePair<int,ChessGridData> pair in m_ChessGridDataDic)
            {
                if (pair.Value.PlayerData != null && pair.Value.PlayerData.ID != player.m_PlayerData.ID)
                {
                    occupiedGridIndex.Add(pair.Key);
                }
            }

            return occupiedGridIndex;
        }

        /// <summary>
        /// 判断索引对应的单元格是否被占用了
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsGridOccupied(int index)
        {
            if (m_ChessGridDataDic.TryGetValue(index, out ChessGridData chessGridData))
            {
                return chessGridData?.PlayerData != null;
            }

            return false;
        }
    }
}