using System.Collections.Generic;
using Script.GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class ChessboardView: BaseMonoBehavior
    {

        #region 序列化属性

        public UIList m_Chessboard;

        public Text m_SelfName;
        public GameObject m_SelfMark;
        
        
        public Text m_OppoName;
        public GameObject m_OppoMark;
        
        
        #endregion

        #region 生命周期

        protected override void OverrideEnable()
        {
            base.OverrideEnable();
            AttachEvent(EventKey.SwitchPlayer, OnSwitchPlayer);
            SetChessboardGrid(ChessBoardData.GetInstance().m_ChessGridDataDic);
            m_SelfName.text = ChessBoardData.GetInstance().m_SelfPlayer.m_PlayerData.ID.ToString();
            m_OppoName.text = ChessBoardData.GetInstance().m_OppoPlayer.m_PlayerData.ID.ToString();
            SetMark();
        }

        protected override void OverrideStart()
        {
            base.OverrideStart();
            
        }

       

        protected override void OverrideUpdate()
        {
            base.OverrideUpdate();
            
        }

        #endregion
        
        
        #region 事件处理

        private void OnSwitchPlayer(IEvent ie)
        {
            SetMark();
        }
        
        #endregion


        #region 内部调用

        private void SetChessboardGrid(Dictionary<int, ChessGridData> mChessGridDataDic)
        {
            m_Chessboard.SetDatas(mChessGridDataDic.Values);
        }

        private void SetMark()
        {
            m_SelfMark.SetActive(ChessBoardData.GetInstance().GetCurrentPlayer() == ChessBoardData.GetInstance().m_SelfPlayer);
            m_OppoMark.SetActive(ChessBoardData.GetInstance().GetCurrentPlayer() == ChessBoardData.GetInstance().m_OppoPlayer);
        }

        #endregion
        
    }
}