using System;
using Script.GameCore;
using UnityEngine;

namespace Script.GameCore
{
    public class Main : MonoBehaviour
    {
        #region 内部变量

        private GameManager m_GameManager;
        
        private UIManager m_UIManager;
        
        private EventManager m_EventManager;
        
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

        private void OnDisable()
        {
            if (m_GameManager != null)
            {
                m_GameManager.OnDisable();
            }

            if (m_UIManager != null)
            {
                m_UIManager.OnDisable();
            }
        }
    }
}