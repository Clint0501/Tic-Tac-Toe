using System;
using Script.GameCore;
using Script.GameCore.Util;
using UnityEngine;

namespace Script.GameCore
{
    public class Main : MonoBehaviour
    {
        private bool IsResMapLoaded = false;
        private void Start()
        {
            //游戏UI管理器
            UIManager.GetInstance().Start();
            //资源加载器
            IsResMapLoaded = ResManager.GetInstance().Init();
            if (!IsResMapLoaded)
            {
                UIUitl.ShowErrorTips("资源加载出错");
                return;
            }
            //游戏单局管理器
            GameManager.GetInstance().Start();
            
            
            EventUtil.SendOpenViewEvent("StartMenuView");
        }

        private void Update()
        {
            if (!IsResMapLoaded) return;
            GameManager.GetInstance().Update();
            UIManager.GetInstance().Update();
        }

        private void OnDisable()
        {
            GameManager.GetInstance().OnDisable();
            UIManager.GetInstance().OnDisable();
        }
    }
}