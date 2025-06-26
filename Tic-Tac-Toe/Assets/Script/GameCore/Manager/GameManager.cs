using System;
using System.Collections.Generic;
using Script.Common;
using Script.GameCore.PlayerCtrl;
using Script.GameCore.Util;
using UnityEngine;

namespace Script.GameCore
{
    public class GameManager : Singleton<GameManager>
    {

        #region 静态变量

        private static readonly List<List<int>> s_GameWinCombo = new List<List<int>>()
        {
            new List<int>(){0,1,2},
            new List<int>(){3,4,5},
            new List<int>(){6,7,8},
            new List<int>(){0,3,6},
            new List<int>(){1,4,7},
            new List<int>(){2,5,8},
            new List<int>(){0,4,8},
            new List<int>(){2,4,6}
        };

        public static readonly Dictionary<int, List<List<int>>> s_Index2Combo = new Dictionary<int, List<List<int>>>()
        {
            { 0, new List<List<int>> { s_GameWinCombo[0], s_GameWinCombo[3], s_GameWinCombo[6] } },
            { 1, new List<List<int>> { s_GameWinCombo[0], s_GameWinCombo[4] } },
            { 2, new List<List<int>> { s_GameWinCombo[0], s_GameWinCombo[5], s_GameWinCombo[7] } },
            { 3, new List<List<int>> { s_GameWinCombo[1], s_GameWinCombo[3] } },
            { 4, new List<List<int>> { s_GameWinCombo[1], s_GameWinCombo[4], s_GameWinCombo[6], s_GameWinCombo[7] } },
            { 5, new List<List<int>> { s_GameWinCombo[1], s_GameWinCombo[5] } },
            { 6, new List<List<int>> { s_GameWinCombo[2], s_GameWinCombo[3], s_GameWinCombo[7] } },
            { 7, new List<List<int>> { s_GameWinCombo[2], s_GameWinCombo[4] } },
            { 8, new List<List<int>> { s_GameWinCombo[2], s_GameWinCombo[5], s_GameWinCombo[6] } },
        };

        #endregion
        
        #region 内部变量

        private bool m_IsGameStart = false;
        
        private bool m_IsWaitingChessDown = false;

        private bool m_IsGameChecking = false;
 
        #endregion

        #region 外部调用
        #region 生命周期
        // Start is called before the first frame update
        public void Start()
        {
            EventManager.GetInstance().AttachEvent(EventKey.GameStart, OnGameStart);
            EventManager.GetInstance().AttachEvent(EventKey.Continue, OnGameContinue);
            EventManager.GetInstance().AttachEvent(EventKey.PlayerChessDown, OnPlayerChessDown);
        }

        

        // Update is called once per frame
        public void Update()
        {
            if (!m_IsGameStart) return;
            AIUpate();
            GameCheck();
        }

        public void OnDisable()
        {
            EventManager.GetInstance().DetachEvent(EventKey.GameStart, OnGameStart);
            EventManager.GetInstance().DetachEvent(EventKey.Continue, OnGameContinue);
            EventManager.GetInstance().DetachEvent(EventKey.PlayerChessDown, OnPlayerChessDown);
        }

        #endregion
        
        #endregion
        
        #region 内部调用

        /// <summary>
        /// 开始对局
        /// </summary>
        private void HandleGameStart(GameModeEnum mode)
        {
            m_IsWaitingChessDown = true;
            ChessBoardData.GetInstance().Init(mode);
        }

        private float m_AIThinkingTime = 1.5f;
        private float m_Counter = 0;
        private void AIUpate()
        {
            if (ChessBoardData.GetInstance().GetCurrentPlayer() is AIPlayer aiPlayer)
            {
                //AI让他缓几秒再执行，不然太快了，体验不好
                m_Counter += Time.deltaTime;
                if (m_Counter >= m_AIThinkingTime)
                {
                    aiPlayer.Update();
                    m_Counter = 0;
                }
                
            }
        }
        
        
        /// <summary>
        /// 对局结果检查
        /// </summary>
        private void GameCheck()
        {
            if (!m_IsGameChecking) return;
            GameResultEnum resultEnum = HasResult(ChessBoardData.GetInstance().m_ChessGridDataDic ,out PlayerData winner);
            if ( resultEnum == GameResultEnum.None)
            {
                SwitchPlayer();
            }
            else
            {
                m_IsGameStart = false;
                EventUtil.SendCloseViewEvent("ChessboardView");
                EventUtil.SendOpenViewEvent("ResultView", true, winner);
            }
            m_IsGameChecking = false;
        }

        /// <summary>
        /// 切换玩家
        /// </summary>
        private void SwitchPlayer()
        {
            ChessBoardData.GetInstance().SwitchPlayer();
            EventManager.GetInstance().DispatchBaseEvent(EventKey.SwitchPlayer);
        }

        /// <summary>
        /// 检查结果并返回胜者
        /// </summary>
        /// <param name="mChessGridDataList"></param>
        /// <param name="winner"></param>
        /// <returns></returns>
        private GameResultEnum HasResult(Dictionary<int, ChessGridData> mChessGridDataList, out PlayerData winner)
        {
            List<int> ignoreIndexs = new List<int>();
            for (int i = 0; i < s_Index2Combo.Count; i++)
            {
                List<List<int>> combos = s_Index2Combo[i];
                foreach (List<int> combo in combos)
                {
                    bool isIgnoreCombo = false;
                    foreach (int ignoreIndex in ignoreIndexs)
                    {
                        if (combo.Contains(ignoreIndex))
                        {
                            isIgnoreCombo = true;
                            break;
                        }
                    }

                    if (isIgnoreCombo) continue;
                    bool hasResult = false;
                    string id = String.Empty;
                    for (int j = 0; j < combo.Count; j++)
                    {
                        int index = combo[j];
                        if (mChessGridDataList[index].PlayerData == null)
                        {
                            ignoreIndexs.Add(index);
                            break;
                        }

                        if (id == String.Empty)
                        {
                            id = mChessGridDataList[index].PlayerData.ID;
                            continue;
                        }

                        if (mChessGridDataList[index].PlayerData.ID != id)
                        {
                            hasResult = false;
                            break;
                        }

                        if (j == combo.Count - 1)
                        {
                            hasResult = true;
                        }
                    }

                    if (hasResult)
                    {
                        if (id == ChessBoardData.GetInstance().m_SelfPlayer.m_PlayerData.ID)
                        {
                            winner = ChessBoardData.GetInstance().m_SelfPlayer.m_PlayerData;
                            return GameResultEnum.Win;
                        }

                        if (id == ChessBoardData.GetInstance().m_OppoPlayer.m_PlayerData.ID)
                        {
                            winner = ChessBoardData.GetInstance().m_OppoPlayer.m_PlayerData;
                            return GameResultEnum.Lose;
                        }
                    }
                }
            }

            winner = null;
            return ignoreIndexs.Count == 0 ? GameResultEnum.Draw : GameResultEnum.None;
        }

        /// <summary>
        /// 玩家落子
        /// </summary>
        /// <param name="ie"></param>
        private void OnPlayerChessDown(IEvent ie)
        {
            if (!m_IsGameStart) return;
            if(ie is PlayerChessDownEvent playerChessDownEvent)
            {
                int gridIndex = playerChessDownEvent.m_GridIndex;
                PlayerData playerGridData = playerChessDownEvent.m_PlayerData;
                
                //不是当前回合者落子不算数
                if (playerGridData.ID != ChessBoardData.GetInstance().GetCurrentPlayer().m_PlayerData.ID)
                {
                    return;
                }
                if (ChessBoardData.GetInstance().IsGridOccupied(gridIndex))
                {
                    UIUitl.ShowErrorTips("请落在空白的棋格子上");
                    return;
                }

                ChessBoardData.GetInstance().SetChessGridData(playerGridData, gridIndex);
                EventUtil.SendChessDoneEvent(gridIndex);
                m_IsGameChecking = true;
            }
        }

        /// <summary>
        /// 游戏开始的事件处理
        /// </summary>
        /// <param name="ie"></param>
        private void OnGameStart(IEvent ie)
        {
            if (ie is not StartGameEvent startGameEvent) return;
            HandleGameStart(startGameEvent.m_GameMode);

            m_IsGameStart = true;
            EventUtil.SendOpenViewEvent("ChessboardView", true);
        }
        
        /// <summary>
        /// 游戏继续的事件处理
        /// </summary>
        /// <param name="ie"></param>
        private void OnGameContinue(IEvent ie)
        {
            if(ie is BaseEvent be && be.m_Key == EventKey.Continue)
            {
                ChessBoardData.GetInstance().Continue();
                m_IsWaitingChessDown = true;
                m_IsGameChecking = false;
                m_IsGameStart = true;
                EventUtil.SendOpenViewEvent("ChessboardView",true);
            }
        }

        #endregion
        
    }
}

