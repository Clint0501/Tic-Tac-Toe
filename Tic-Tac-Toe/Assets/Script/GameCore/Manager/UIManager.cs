using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Script.GameCore
{
    public class UIManager : Common.Singleton<UIManager>
    {
        #region 内部常量

        private const string UI_PATH_FORMAT = "Assets/BundleResources/UI/{0}.prefab";

        #endregion
        
        #region 内部变量

        private Dictionary<string,BaseMonoBehavior> m_UICache = new Dictionary<string,BaseMonoBehavior>();
        
        private Dictionary<string,BaseMonoBehavior> m_ActiveUIDic = new Dictionary<string,BaseMonoBehavior>();

        private Transform m_UIRoot;

        private float m_ClearUICacheCountDown = 30;

        private float m_Timer = 0;

        private bool m_IsClearingCache = false;
        private Queue<string> m_CacheToClear = new Queue<string>();
        #endregion


        #region 生命周期

        public void Start()
        {
            m_UIRoot = GameObject.Find("Canvas").transform;
            EventManager.GetInstance().AttachEvent(EventKey.OpenView, OnOpenView);
            EventManager.GetInstance().AttachEvent(EventKey.CloseView, OnCloseView);
        }
        

        public void Update()
        {
            m_Timer += Time.deltaTime;
            if (m_Timer >= m_ClearUICacheCountDown)
            {
                StartClearUICache();
                m_Timer = 0;
            }

            // 处理待清理的缓存
            ProcessCacheClearing();
        }

        private void StartClearUICache()
        {
            if (m_IsClearingCache) return;
            
            m_IsClearingCache = true;
            m_CacheToClear.Clear();
            
            // 将需要清理的UI名称加入队列
            foreach (var kvp in m_UICache)
            {
                m_CacheToClear.Enqueue(kvp.Key);
            }
        }

        private void ProcessCacheClearing()
        {
            if (!m_IsClearingCache || m_CacheToClear.Count == 0) return;

            // 每帧只清理一个UI，避免卡顿
            string viewName = m_CacheToClear.Dequeue();
            if (m_UICache.TryGetValue(viewName, out BaseMonoBehavior view))
            {
                Object.Destroy(view);
                m_UICache.Remove(viewName);
            }

            if (m_CacheToClear.Count == 0)
            {
                m_IsClearingCache = false;
            }
        }

        public void OnDisable()
        {
            EventManager.GetInstance().DetachEvent(EventKey.OpenView, OnOpenView);
            EventManager.GetInstance().DetachEvent(EventKey.CloseView, OnCloseView);
        }

        #endregion
        
        #region 内部调用

        /// <summary>
        /// 关闭某个UI，放进缓存中
        /// </summary>
        /// <param name="viewName"></param>
        private void OnCloseView(IEvent ie)
        {
            if (ie is not CloseUIEvent closeUIEvent) return;
            string viewName = closeUIEvent.m_ViewName;
            if (m_ActiveUIDic.TryGetValue(viewName, out BaseMonoBehavior view))
            {
                view.gameObject.SetActive(false);
                m_UICache.Add(viewName, view);
                m_ActiveUIDic.Remove(viewName);
            }
        }

        /// <summary>
        /// 打开某个UI
        /// </summary>
        /// <param name="viewName"></param>
        private void OnOpenView(IEvent ie)
        {
            if (ie is not OpenUIEvent openUIEvent) return;
            string viewName = openUIEvent.m_ViewName;
            object[] args = openUIEvent.m_Args;

            if (!m_UICache.TryGetValue(viewName, out BaseMonoBehavior view))
            {
#if UNITY_EDITOR
                GameObject ui = AssetDatabase.LoadAssetAtPath<GameObject>(string.Format(UI_PATH_FORMAT, viewName));
                if (!ui)
                {
                    Debug.LogError($"没有找到{viewName}界面");
                    return;
                }
                GameObject uiInst = Object.Instantiate(ui, m_UIRoot, false);
                if (!uiInst)
                {
                    Debug.LogError($"实例化{viewName}界面失败");
                    return;
                }
                view = uiInst.GetComponent<BaseMonoBehavior>();
#else
                //TODO 正式环境下动态加载资源的逻辑
#endif
            }

            if (view != null)
            {
                view.gameObject.SetActive(true);
                view.InitViewData(args);
                m_ActiveUIDic.Add(viewName, view);
            }
        }
        
        
        #endregion

    }
}