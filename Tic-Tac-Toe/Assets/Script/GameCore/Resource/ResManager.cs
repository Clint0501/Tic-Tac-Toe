using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Script.Common;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Script.GameCore
{
    public class ResManager : Singleton<ResManager>
    {
        private ResMap m_ResMap = new ResMap();
        
        private Dictionary<string, Object> m_CachedAssets = new Dictionary<string, Object>();

        private Dictionary<string, AssetBundle> m_CachedAssetBundles = new Dictionary<string, AssetBundle>();
        
        public static readonly string s_ResMapJsonPath = Application.persistentDataPath + "/ResMap.json";
        public static readonly string s_LocalBundlesPath = Application.persistentDataPath + "/AssetBundles/";
        public bool Init()
        {
            m_ResMap  = JsonUtility.FromJson<ResMap>(s_ResMapJsonPath);
            if (m_ResMap == null || m_ResMap.m_AssetInfoMap.Count == 0)
            {
                Debug.LogError("ResMap加载失败");
                return false;
            }

            return true;
        }
        
        public T LoadAsset<T>(string assetName) where T : Object
        {
            if (m_CachedAssets.TryGetValue(assetName, out Object asset))
            {
                return asset as T;
            }

            T obj = null;
            if (!m_ResMap.m_AssetInfoMap.TryGetValue(assetName, out AssetInfo info))
            {
                Debug.LogError($"未能找到{assetName}资源信息");
                return null;
            }
#if UNITY_EDITOR
            obj = AssetDatabase.LoadAssetAtPath<T>(info.m_AssetPath);
#else    
            obj = LoadAssetFromBundle<T>(info);
            
#endif
            if (obj)
            {
                m_CachedAssets.Add(assetName, obj);
            }
            return obj;
        }

        private T LoadAssetFromBundle<T>(AssetInfo assetInfo) where T : Object
        {
            AssetBundle ab = LoadBundle(assetInfo);
            if (ab == null)
            {
                Debug.LogError($"{assetInfo.m_BundleName}AB包获取失败");
                return null;
            }

            T obj = ab.LoadAsset<T>(assetInfo.m_AssetName);
            if (!obj)
            {
                Debug.LogError($"{assetInfo.m_AssetName}不在{assetInfo.m_BundleName}中");
            }

            return obj;
        }

        private AssetBundle LoadBundle(AssetInfo assetInfo)
        {
            if (m_CachedAssetBundles.TryGetValue(assetInfo.m_BundleName, out AssetBundle bundle))
            {
                return bundle;
            }

            foreach (string dependency in assetInfo.m_Dependencies)
            {
                LoadBundle(dependency);
            }

            return LoadBundle(assetInfo.m_BundleName);
        }

        private AssetBundle LoadBundle(string bundleName)
        {
            // 加载Bundle
            string bundlePath = Path.Combine(s_LocalBundlesPath, bundleName);
            AssetBundle ab = null;
            if (File.Exists(bundlePath))
            {
                ab = AssetBundle.LoadFromFile(bundlePath);
                if (ab != null)
                {
                    m_CachedAssetBundles[bundleName] = ab;
                }
            }
        
            return ab;
        }
    }
}