using System.Collections;
using System.Collections.Generic;
using Script.GameCore;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Script.UI
{
    public class ErrorTipsView : BaseMonoBehavior
    {
        #region 序列化属性

        public Text m_Text;

        #endregion
        
        
        public override void InitViewData(object[] args)
        {
            base.InitViewData(args);
            if (args.Length > 0)
            {
                m_Text.text = args[0].ToString();
            }
        }

        protected override void OverrideStart()
        {
            base.OverrideStart();
            m_Text.rectTransform.DOAnchorPosY(170, 1.2f);
        }
    }
}
