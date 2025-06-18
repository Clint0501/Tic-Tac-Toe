using System;
using System.Collections.Generic;
using Script.Common;
using Script.GameCore.Util;
using UnityEngine;

namespace Script.GameCore
{
    public class GameManager : Singleton<GameManager>
    {
    
        #region 生命周期
        // Start is called before the first frame update
        public void Start()
        {
            EventManager.GetInstance().AttachEvent(EventKey.GameStart, OnGameStart);
            EventManager.GetInstance().AttachEvent(EventKey.PlayerChessDown, OnPlayerChessDown);
        }

        private void OnPlayerChessDown(IEvent ie)
        {
            if(ie is PlayerChessDownEvent playerChessDownEvent)
            {
                int gridIndex = playerChessDownEvent.m_GridIndex;
                PlayerData chessGridData = playerChessDownEvent.m_PlayerData;
                if (!ChessBoardData.GetInstance().TrySetChessGridData(chessGridData, gridIndex))
                {
                    UIUitl.ShowErrorTips("请落在空白的棋格子上");
                }
            }
        }

        private void OnGameStart(IEvent ie)
        {
            if (ie is not StartGameEvent startGameEvent) return;
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
        
    }
}

