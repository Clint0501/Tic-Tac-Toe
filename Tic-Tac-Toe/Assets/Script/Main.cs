using System;
using UnityEngine;

namespace Script
{
    public class Main : MonoBehaviour
    {
        #region 内部变量

        private GameManager m_GameManager;
        
        private UIManager m_UIManager;
        
        #endregion
        
        private void Start()
        {
            //游戏单局管理器
            m_GameManager = GameManager.GetInstance();
            m_GameManager.Start();
            
            //游戏UI管理器
            m_UIManager = UIManager.GetInstance();
            m_UIManager.Start();
        }
        
        private void Update()
        {
            if (m_GameManager != null)
            {
                m_GameManager.Update();
            }

            if (m_UIManager != null)
            {
                m_UIManager.Update();
            }
        }
    }
}