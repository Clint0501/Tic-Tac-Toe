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

        private void OnPlayerChessDown(object obj)
        {
            throw new NotImplementedException();
        }

        private void OnGameStart(object obj)
        {
            if(obj is GameModeEnum _enum)
            {
                switch (_enum)
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

        private void HandleAIPlayerGameStart()
        {
            throw new NotImplementedException();
        }

        private void HandleTwoPlayerGameStart()
        {
            throw new NotImplementedException();
        }

        // Update is called once per frame
        public void Update()
        {
        
        }

        public void OnDisable()
        {
            
        }

        #endregion
    
        #region 内部调用

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

        public void AttachEvent(EventKey eventName, Action<object> action)
        {
            throw new NotImplementedException();
        }

        public void DetachAllEvent()
        {
            throw new NotImplementedException();
        }
    }
}

