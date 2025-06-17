using System;
using System.Collections.Generic;
using Script.Common;
using UnityEngine;

namespace Script.GameCore
{
    public class GameManager : Singleton<GameManager>
    {

        #region 内部变量

        private PlayerData m_Player1;
        private PlayerData m_Player2;
    
        List<ChessGridData>  m_ChessGridList;

        #endregion
    
        #region 生命周期
        // Start is called before the first frame update
        public void Start()
        {
            m_ChessGridList = new List<ChessGridData>(9);
            EventManager.GetInstance().AttachEvent(EventKey.GameStart, OnGameStart);
            EventManager.GetInstance().AttachEvent(EventKey.PlayerChessDown, OnPlayerChessDown);
        }

        private void OnPlayerChessDown(IEvent ie)
        {
            if(ie is PlayerChessDownEvent playerChessDownEvent)
            {
                int gridIndex = playerChessDownEvent.m_GridIndex;
                PlayerData chessGridData = playerChessDownEvent.m_PlayerData;
                if (m_ChessGridList[gridIndex].PlayerData == null)
                {
                    m_ChessGridList[gridIndex].PlayerData = chessGridData;
                }
                else
                {
                    OpenUIEvent evt = new OpenUIEvent
                    {
                        m_ViewName = "ErrorTipsView",
                        m_Args = new object[]{ "请下在正确的格子上" }
                    };
                    EventManager.GetInstance().DispatchEvent(evt);
                }
            }
        }

        private void OnGameStart(IEvent ie)
        {
            if(ie is StartGameEvent startGameEvent)
            {
                switch (startGameEvent.m_GameMode)
                {
                    case GameModeEnum.TwoPlayer:
                        HandleTwoPlayerGameStart();
                        break;
                    case  GameModeEnum.AIPlayer:
                        HandleAIPlayerGameStart();
                        break;
                    default:
                        break;
                    
                }
            }
        }

       

        // Update is called once per frame
        public void Update()
        {
        
        }

        public void OnDisable()
        {
            EventManager.GetInstance().DetachEvent(EventKey.GameStart, OnGameStart);
            EventManager.GetInstance().DetachEvent(EventKey.PlayerChessDown, OnPlayerChessDown);
        }

        #endregion
    
        #region 内部调用

        private void HandleAIPlayerGameStart()
        {
            throw new NotImplementedException();
        }

        private void HandleTwoPlayerGameStart()
        {
            throw new NotImplementedException();
        }
        
        private void GameCheck()
        {
        
        }
        #endregion
    
        #region 外部调用

        public void SetPlayer1(PlayerData player)
        {
            m_Player1 = player;
        }

        public void SetPlayer2(PlayerData player)
        {
            m_Player2 = player;
        }
        #endregion
        
    }
}

