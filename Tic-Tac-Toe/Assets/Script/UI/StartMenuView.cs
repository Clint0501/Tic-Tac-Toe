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
            StartGameEvent startGameEvent  = new StartGameEvent
            {
                m_GameMode = GameModeEnum.TwoPlayer
            };
            EventManager.GetInstance().DispatchEvent(startGameEvent);
        }

        private void OnAIGameStart()
        {
            StartGameEvent startGameEvent  = new StartGameEvent
            {
                m_GameMode = GameModeEnum.AIPlayer
            };
            EventManager.GetInstance().DispatchEvent(startGameEvent);
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
