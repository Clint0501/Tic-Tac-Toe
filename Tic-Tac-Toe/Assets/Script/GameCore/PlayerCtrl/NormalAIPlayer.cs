using System.Collections.Generic;

namespace Script.GameCore.PlayerCtrl
{
    [Player(GameModeEnum.NormalAIPlayer)]
    public class NormalAIPlayer : EasyAIPlayer
    {
        public NormalAIPlayer()
        {
            m_PlayerData = new PlayerData("普通人机");
        }
        //防住对面的连子
        protected override int GetPressIndex()
        {
            int index = GetDefendIndex();
            if (index == -1)
            {
                index =  GetEmptyIndex();
            }

            return index;
        }

        protected int GetDefendIndex()
        {
            int resultIndex = -1;
            for (int i = 0; i < GameManager.s_Index2Combo.Count; i++)
            {
                List<List<int>> combos = GameManager.s_Index2Combo[i];
                foreach (List<int> combo in combos)
                {
                    int otherCount = 0;
                    int curComboResultIndex = -1;
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
                            //只要有一个格子是自己的，这个组合就可以不用检查了，安全了.重置
                            if (ChessBoardData.GetInstance().m_ChessGridDataDic[index].PlayerData.ID == this.m_PlayerData.ID)
                            {
                                curComboResultIndex = -1;
                                break;
                            }
                                

                            //如果有一个格子
                            if (ChessBoardData.GetInstance().m_ChessGridDataDic[index].PlayerData.ID != this.m_PlayerData.ID)
                            {
                                otherCount++;
                            }
                        }

                    }
                    //当前组合如果有两个其他人的子，切有空位，这个空位就是我们能要的防守点
                    if (otherCount == 2 && curComboResultIndex != -1)
                    {
                        resultIndex = curComboResultIndex;
                    }
                }
            }

            return resultIndex;
        }

        
    }
}