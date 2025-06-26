using System;
using Script.GameCore;
using Script.GameCore.Util;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UI
{
    public class StartMenuView : BaseMonoBehavior
    {

        #region 序列化属性

        public GameObject m_MainMenu;
        
        public Button m_AIPlayerButton;
        
        public Button m_TwoPlayerButton;
        
        public GameObject m_SubMenu;
        
        public Button m_EasyAIPlayerButton;
        
        public Button m_NormalAIPlayerButton;
        
        public Button m_HardAIPlayerButton;
        
        public Button m_ReturnButton;
        
        #endregion
        
        #region 生命周期
        
        protected override void OverrideEnable()
        {
            base.OverrideEnable();
            m_AIPlayerButton.onClick.AddListener(OnAIGameStart);
            m_TwoPlayerButton.onClick.AddListener(OnTwoPlayerGameStart);
            m_ReturnButton.onClick.AddListener(OnReturn);
            m_EasyAIPlayerButton.onClick.AddListener(OnEasyAIPlayerButton);
            m_NormalAIPlayerButton.onClick.AddListener(OnNormalAIPlayerButton);
            m_HardAIPlayerButton.onClick.AddListener(OnHardAIPlayerButton);
            SetMenu(true);
        }
        
        protected override void OverrideStart()
        {
            base.OverrideStart();
        }

        protected override void OverrideUpdate()
        {
            base.OverrideUpdate();
        }

        protected override void OverrideDisable()
        {
            base.OverrideDisable();
            m_AIPlayerButton.onClick.RemoveAllListeners();
            m_TwoPlayerButton.onClick.RemoveAllListeners();
            m_ReturnButton.onClick.RemoveAllListeners();
            m_EasyAIPlayerButton.onClick.RemoveAllListeners();
            m_NormalAIPlayerButton.onClick.RemoveAllListeners();
            m_HardAIPlayerButton.onClick.RemoveAllListeners();
        }

        protected override void OverrideDestroy()
        {
            base.OverrideDestroy();
        }
        #endregion


        #region 内部调用

        private void OnTwoPlayerGameStart()
        {
            EventUtil.SendStartGameEvent(GameModeEnum.TwoPlayer);
        }

        private void OnAIGameStart()
        {
            SetMenu(false);
            
        }
        
        private void OnHardAIPlayerButton()
        {
            EventUtil.SendStartGameEvent(GameModeEnum.HardAIPlayer);
        }

        private void OnNormalAIPlayerButton()
        {
            EventUtil.SendStartGameEvent(GameModeEnum.NormalAIPlayer);
        }

        private void OnEasyAIPlayerButton()
        {
            EventUtil.SendStartGameEvent(GameModeEnum.EasyAIPlayer);
        }

        private void SetMenu(bool isMain)
        {
            m_MainMenu.SetActive(isMain);
            m_SubMenu.SetActive(!isMain);
        }
        
        private void OnReturn()
        {
            SetMenu(true);
        }

        #endregion
    }
}
