using System.Collections;
using System.Collections.Generic;
using Script.GameCore;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Script.GameCore.Util;
using Unity.VisualScripting;

namespace Script.UI
{
    public class ErrorTipsView : BaseMonoBehavior
    {
        #region 序列化属性

        public Text m_Text;

        public RectTransform m_Bg;

        [InspectorLabel("界面自动关闭倒计时")]
        public float m_CloseCountDown = 2.0f;
        #endregion

        #region 私有变量

        private float m_Counter = 0;

        private bool m_HasClose = false;


        #endregion 
        
        #region 初始化数据

        public override void InitViewData(object[] args)
        {
            base.InitViewData(args);
            if (args.Length > 0)
            {
                m_Text.text = args[0].ToString();
            }
        }

        #endregion
        
        #region 生命周期
        protected override void OverrideEnable()
        {
            base.OverrideEnable();
            if (m_Bg != null)
            {
                m_Bg.DOAnchorPosY(170, 1.2f);
            }
        }

        protected override void OverrideUpdate()
        {
            base.OverrideUpdate();
            m_Counter += Time.deltaTime;
            if (m_Counter >= m_CloseCountDown)
            {
                EventUtil.SendCloseViewEvent("ErrorTipsView");
            }
        }


        protected override void OverrideDisable()
        {
            base.OverrideDisable();
            m_Counter = 0;
            m_Bg.DOAnchorPosY(0, 0);
        }
        
        #endregion
    }
}
