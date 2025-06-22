using System.Collections.Generic;
using Script.GameCore;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class ChessboardView: BaseMonoBehavior
    {

        #region 界面组件

        public UIList m_Chessboard;

        public Text m_ResulText;
        
        #endregion

        #region 生命周期

        protected override void OverrideEnable()
        {
            base.OverrideEnable();
            AttachEvent(EventKey.SwitchPlayer,OnSwitchPlayer);
            AttachEvent(EventKey.GameOver,OnGameOver);
        }

        protected override void OverrideStart()
        {
            base.OverrideStart();
            SetChessboardGrid(ChessBoardData.GetInstance().m_ChessGridDataDic);
        }

        private void SetChessboardGrid(Dictionary<int, ChessGridData> mChessGridDataDic)
        {
            m_Chessboard.SetDatas(mChessGridDataDic.Values);
        }

        protected override void OverrideUpdate()
        {
            base.OverrideUpdate();
            
        }

        #endregion
        
        
        #region 事件处理
        
        private void OnGameOver(IEvent ie)
        {
            if (ie is GameOverEvent evt)
            {
                if (m_ResulText != null)
                {
                    m_ResulText.text = string.Format($"{evt.m_PlayerData.ID}胜利");
                }
            }
        }

        private void OnSwitchPlayer(IEvent ie)
        {
            
        }
        
        #endregion
        
    }
}