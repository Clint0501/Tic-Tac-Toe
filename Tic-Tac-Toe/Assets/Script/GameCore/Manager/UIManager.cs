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
            
        }

        public void Update()
        {
            
        }

        #endregion
        
        #region 外部调用

        /// <summary>
        /// 打开某个UI
        /// </summary>
        /// <param name="viewName"></param>
        public void OpenView(string viewName,params object[] args)
        {
            
        }
        #endregion

    }
}