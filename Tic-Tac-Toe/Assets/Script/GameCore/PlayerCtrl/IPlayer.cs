using System;
using System.Collections.Generic;
using System.Linq;
using Script.GameCore.Util;
using UnityEngine.PlayerLoop;

namespace Script.GameCore.PlayerCtrl
{
    public interface IPlayer
    {
        void Action();
    }

    public class BasePlayer : IPlayer
    {
        public PlayerData m_PlayerData { get; set; }

        public int Index { get; set; }
        public void Action()
        {
            Index = GetPressIndex();
            EventUtil.SendChessDownEvent(m_PlayerData, Index);
        }

        protected virtual int GetPressIndex()
        {
            return Index;
        }
    }

    public class AIPlayer : BasePlayer
    {
        /// <summary>
        /// 轮到AI时他自己执行
        /// </summary>
        public void Update()
        {
            Action();
        }
    }
    
    public class EasyAIPlayer : AIPlayer
    {
        protected override int GetPressIndex()
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
    
    public class HardAIPlayer : AIPlayer
    {
        protected override int GetPressIndex()
        {
            //TODO 找到能获胜的最佳落点
            // int maxComboCount = Int32.MinValue;
            // int index = 0;
            // foreach (KeyValuePair<int,List<List<int>>> pair in GameManager.s_Index2Combo)
            // {
            //     if (pair.Value.Count > maxComboCount)
            //     {
            //         maxComboCount = pair.Value.Count;
            //         index = pair.Key;
            //     }
            // }
            return 0;
        }
    }
    
}