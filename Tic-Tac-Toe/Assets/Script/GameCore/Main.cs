using System;
using Script.GameCore;
using Script.GameCore.Util;
using UnityEngine;

namespace Script.GameCore
{
    public class Main : MonoBehaviour
    {
        private void Start()
        {
            //游戏单局管理器
            GameManager.GetInstance().Start();
            //游戏UI管理器
            UIManager.GetInstance().Start();
            
            EventUtil.SendOpenViewEvent("StartMenuView");
        }

        private void Update()
        {
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