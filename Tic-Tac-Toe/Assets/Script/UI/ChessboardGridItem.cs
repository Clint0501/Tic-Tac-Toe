using Script.GameCore;
using Script.GameCore.Util;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

namespace Script.UI
{
    public class ChessboardGridItem : UIListElement
    {
        #region 序列化字段

        public Button m_Button;

        public Image m_ImageSelf;

        public Image m_ImageOppo;
        
        #endregion


        #region 生命周期

        protected override void OverrideEnable()
        {
            base.OverrideEnable();
            m_Button.onClick.AddListener(OnButtonClick);
            if (m_ImageSelf)
            {
                m_ImageSelf.gameObject.SetActive(false);
            }

            if (m_ImageOppo)
            {
                m_ImageOppo.gameObject.SetActive(false);
            }
            AttachEvent(EventKey.PlayerChessDone,OnPlayerChessDone);

        }

        protected override void OverrideDisable()
        {
            base.OverrideDisable();
            m_Button.onClick.RemoveAllListeners();
        }

        public override void UpdateData(object data)
        {
            base.UpdateData(data);
            UpdateImage();
        }

        private void UpdateImage()
        {
            if (m_Data is PlayerData playerData)
            {
                bool isSelf = playerData.ID == ChessBoardData.GetInstance().m_SelfPlayer.m_PlayerData.ID;
                m_ImageSelf.gameObject.SetActive(isSelf);
                m_ImageOppo.gameObject.SetActive(!isSelf);
            }
        }

        #endregion

        #region 回调方法

        void OnButtonClick()
        {
            EventUtil.SendChessDownEvent(ChessBoardData.GetInstance().m_SelfPlayer.m_PlayerData, m_Index);
        }
        
        private void OnPlayerChessDone(IEvent ie)
        {
            if (ie is PlayerChessDoneEvent evt && evt.m_GridIndex == m_Index)
            {
                UpdateData(ChessBoardData.GetInstance().GetChessGridData(m_Index));
            }
        }

        #endregion
        
    }
}