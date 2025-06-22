using System;
using System.Collections.Generic;
using System.Linq;
using Script.GameCore;
using UnityEngine;

namespace Script.UI
{
    public class UIList: BaseMonoBehavior
    {
        #region 序列化字段

        public UIListElement m_UIListElement;

        public GameObject m_Root;

        #endregion

        #region 内部变量

        private List<UIListElement>  m_Elements = new List<UIListElement>();

        #endregion
        
        
        #region 外部调用

        #region 生命周期

        protected override void OverrideEnable()
        {
            base.OverrideEnable();
        }

        #endregion

        public void SetDatas(IEnumerable<object> datas)
        {
            m_Elements.Clear();
            foreach (object d in datas)
            {
                UIListElement uiListElement = Instantiate(m_UIListElement,m_Root.transform);
                m_Elements.Add(uiListElement);
                uiListElement.SetData(d, m_Elements.Count - 1);
            }
        }

        #endregion
    }
}