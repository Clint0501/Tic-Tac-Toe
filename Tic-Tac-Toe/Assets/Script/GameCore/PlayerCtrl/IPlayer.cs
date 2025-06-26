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

        private int Index { get; set; }
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
        protected AIPlayer()
        {
            m_PlayerData = new PlayerData("人机");
        }
        /// <summary>
        /// 轮到AI时他自己执行
        /// </summary>
        public void Update()
        {
            Action();
        }

        
    }
}