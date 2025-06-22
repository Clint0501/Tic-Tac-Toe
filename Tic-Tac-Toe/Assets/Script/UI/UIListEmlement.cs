using Script.GameCore;
using UnityEngine;

namespace Script.UI
{
    public class UIListElement : BaseMonoBehavior
    {
        public object m_Data;
        [HideInInspector]
        public int m_Index;
        public void SetData(object data, int index)
        {
            this.m_Data = data;
            this.m_Index = index;
        }

        public virtual void UpdateData(object data)
        {
            this.m_Data = data;
        }
    }
}