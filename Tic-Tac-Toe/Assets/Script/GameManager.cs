using System.Collections;
using System.Collections.Generic;
using Script.Common;
using Script.GameCore;
using UnityEngine;

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
    }

    // Update is called once per frame
    public void Update()
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
   
}
