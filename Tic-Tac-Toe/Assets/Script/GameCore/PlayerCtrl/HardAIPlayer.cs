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
            //先找自己能够获胜的落点
            int index = GetWinIndex();
            if(index == -1)
            {
                //再防守，再找最佳落点
                index = GetDefendIndex();
                if (index == -1)
                {
                    index = GetMostPossibleWinIndex();
                }
            }
            return index;
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
            for (int i = 0; i < GameManager.s_Index2Combo.Count; i++)
            {
                List<List<int>> combos = GameManager.s_Index2Combo[i];
                foreach (List<int> combo in combos)
                {
                    int curComboResultIndex = -1;
                    int selfCount = 0;
                    int emptyCount = 0;
                    foreach (int index in combo)
                    {
                        var playerData = ChessBoardData.GetInstance().m_ChessGridDataDic[index].PlayerData;
                        if (playerData == null)
                        {
                            curComboResultIndex = index;
                            emptyCount++;
                        }
                        else if (playerData.ID == this.m_PlayerData.ID)
                        {
                            selfCount++;
                        }
                    }
                    // 如果有2个自己的子，1个空位，就是获胜点
                    if (selfCount == 2 && emptyCount == 1)
                    {
                        return curComboResultIndex;
                    }
                }
            }
            return -1;
        }

        
    }

}