using System.Collections.Generic;
using Script.Common;
using UnityEngine;

namespace Script.GameCore
{
    public class UIManager : Singleton<UIManager>
    {
        #region 内部变量

        private Dictionary<string,GameObject> m_UICache = new Dictionary<string,GameObject>();

        #endregion


        #region 生命周期

        public void Start()
        {
            EventManager.GetInstance().AttachEvent(EventKey.OpenView, OnOpenView);
            EventManager.GetInstance().AttachEvent(EventKey.CloseView, OnCloseView);
        }
        

        public void Update()
        {
            
        }


        public void OnDisable()
        {
            EventManager.GetInstance().DetachEvent(EventKey.OpenView, OnOpenView);
            EventManager.GetInstance().DetachEvent(EventKey.CloseView, OnCloseView);
        }

        #endregion
        
        #region 内部调用

        private void OnCloseView(object obj)
        {
            throw new System.NotImplementedException();
        }

        private void OnOpenView(object obj)
        {
            throw new System.NotImplementedException();
        }
        
        /// <summary>
        /// 打开某个UI
        /// </summary>
        /// <param name="viewName"></param>
        private void OpenView(string viewName,params object[] args)
        {
            
        }
        
        /// <summary>
        /// 打开某个UI
        /// </summary>
        /// <param name="viewName"></param>
        private void CloseView(string viewName,params object[] args)
        {
            
        }
        #endregion

    }
}