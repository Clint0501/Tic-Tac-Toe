using System.Collections;
using System.Collections.Generic;
using Script.GameCore;
using Script.GameCore.Util;
using UnityEngine;
using UnityEngine.UI;

public class ResultView : BaseMonoBehavior
{
    #region 序列化属性

    public Button m_Back;
    
    public Button m_Continue;

    public Text m_ResultText;

    #endregion

    #region 初始化数据

    public override void InitViewData(object[] args)
    {
        base.InitViewData(args);
        if (args.Length > 0)
        {
            PlayerData winner = args[0] as PlayerData;
            m_ResultText.text = winner != null ? $"{winner.ID}获胜" : "打成平手";
        }
    }

    #endregion
    
    #region 生命周期

    protected override void OverrideEnable()
    {
        base.OverrideEnable();
        m_Back.onClick.AddListener(BackToMenu);
        m_Continue.onClick.AddListener(Continue);
    }

    protected override void OverrideDisable()
    {
        base.OverrideDisable();
        m_Back.onClick.RemoveAllListeners();
        m_Continue.onClick.RemoveAllListeners();
    }

    #endregion

    #region 回调

    private void Continue()
    {
        EventManager.GetInstance().DispatchBaseEvent(EventKey.Continue);
        EventUtil.SendCloseViewEvent("ResultView");
    }

    private void BackToMenu()
    {
        EventUtil.SendOpenViewEvent("StartMenuView",true);
    }


    #endregion

}
