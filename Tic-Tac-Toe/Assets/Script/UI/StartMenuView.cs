using System;
using Script.GameCore;
using UnityEngine.UI;

namespace Script.UI
{
    public class StartMenuView : BaseMonoBehavior
    {
        public Button m_AIPlayerButton;
        
        public Button m_TwoPlayerButton;
        protected override void OverrideAwake()
        {
            base.OverrideAwake();
            m_AIPlayerButton.onClick.AddListener(OnAIGameStart);
            m_TwoPlayerButton.onClick.AddListener(OnTwoPlayerGameStart);
        }

        private void OnTwoPlayerGameStart()
        {
            EventManager.GetInstance().DispatchEvent(EventKey.GameStart, GameModeEnum.TwoPlayer);
        }

        private void OnAIGameStart()
        {
            EventManager.GetInstance().DispatchEvent(EventKey.GameStart, GameModeEnum.AIPlayer);
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
        }

        protected override void OverrideDestroy()
        {
            base.OverrideDestroy();
        }
    }
}
