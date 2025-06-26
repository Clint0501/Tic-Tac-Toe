using System.Collections.Generic;
using System.Linq;
using UnityEngine.Rendering.VirtualTexturing;

namespace Script.GameCore.PlayerCtrl
{
    [Player(GameModeEnum.HardAIPlayer)]
    public class HardAIPlayer : NormalAIPlayer
    {
        public HardAIPlayer()
        {
            m_PlayerData = new PlayerData("困难人机");
        }
        protected override int GetPressIndex()
        {
            //先防守，再找最佳落点
            int index = GetDefendIndex();
            if (index == -1)
            {
                index = GetBestIndex();
            }

            return index;
        }

        /// <summary>
        /// 获取最佳的落点
        /// </summary>
        /// <returns></returns>
        private int GetBestIndex()
        {
            //先找自己能够获胜的落点
            int resultIndex = GetWinIndex();
            //如果没有可以获胜的落点，就找最有可能获胜的落点
            if(resultIndex == -1)
            {
                resultIndex = GetMostPossibleWinIndex();
            }

            return resultIndex;
        }
        /// <summary>
        /// 获取最有可能获胜的落点
        /// </summary>
        /// <returns></returns>
        private int GetMostPossibleWinIndex()
        {
            int resultIndex = -1;
            IOrderedEnumerable<KeyValuePair<int, List<List<int>>>> orderByDescending = GameManager.s_Index2Combo.OrderByDescending(x => x.Value.Count);
            foreach (KeyValuePair<int, List<List<int>>> pair in orderByDescending)
            {
                if (resultIndex == -1)
                {
                    //先获胜组合最多的落点
                    if (!ChessBoardData.GetInstance().IsGridOccupied(pair.Key))
                    {
                        resultIndex = pair.Key;
                        break;
                    }
                
                    List<List<int>> curIndexAllCombos = pair.Value;
                    foreach (List<int> combo in curIndexAllCombos)
                    {
                        foreach (int index in combo)
                        {
                            if (!ChessBoardData.GetInstance().IsGridOccupied(index))
                            {
                                resultIndex = index;
                            }
                            else
                            {
                                if (ChessBoardData.GetInstance().m_ChessGridDataDic[index].PlayerData != null)
                                {
                                    //如果当前组合有其他人的子，这个组合也不要考虑了
                                    if (ChessBoardData.GetInstance().m_ChessGridDataDic[index].PlayerData.ID != this.m_PlayerData.ID)
                                    {
                                        resultIndex = -1;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return resultIndex;
        }

        /// <summary>
        /// 获取能够获胜的落点
        /// </summary>
        /// <returns></returns>
        private int GetWinIndex()
        {
            int resultIndex = -1;
            for (int i = 0; i < GameManager.s_Index2Combo.Count; i++)
            {
                List<List<int>> combos = GameManager.s_Index2Combo[i];
                int curComboResultIndex = -1;
                int otherCount = 0;
                foreach (List<int> combo in combos)
                {
                    bool hasResult = false;
                    
                    for (int j = 0; j < combo.Count; j++)
                    {
                        int index = combo[j];
                        //有空位，可能是目标落点
                        if (ChessBoardData.GetInstance().m_ChessGridDataDic[index].PlayerData == null)
                        {
                            curComboResultIndex = index;
                        }
                        else
                        {
                            //只要有一个各自是自己的，这个组合就可以不用检查了，没戏了
                            if (ChessBoardData.GetInstance().m_ChessGridDataDic[index].PlayerData.ID != this.m_PlayerData.ID)
                                break;
                       
                            //如果有一个格子
                            if (ChessBoardData.GetInstance().m_ChessGridDataDic[index].PlayerData.ID == this.m_PlayerData.ID)
                            {
                                otherCount++;
                            }
                        }
                    }
                }
                //当前组合如果有两个自己的子，切有空位，这个空位就是我们能获胜的点
                if (otherCount == 2 && curComboResultIndex != -1)
                {
                    resultIndex = curComboResultIndex;
                }
            }

            return resultIndex;
        }

        
    }

}