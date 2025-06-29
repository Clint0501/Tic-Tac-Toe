using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Script.Common;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Script.GameCore
{
    public class ResManager : Singleton<ResManager>
    {
        private Dictionary<string, Object> m_CachedAssets = new Dictionary<string, Object>();

        private Dictionary<string, AssetInfo> m_AssetInfoInfoDic = new Dictionary<string, AssetInfo>();
        private Dictionary<string, AssetBundle> m_CachedAssetBundles = new Dictionary<string, AssetBundle>();
        
        public static readonly string s_ResMapXMLPath = Application.streamingAssetsPath + "/ResMap.xml";
        public static readonly string s_LocalBundlesPath = Application.streamingAssetsPath + "/AssetBundles/";
        public bool Init()
        {
            string content = File.ReadAllText(s_ResMapXMLPath);
            if (content == String.Empty)
            {
                Debug.LogError("AssetMap.xml 未找到！");
                return false;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(ResMap));
            using StringReader reader = new StringReader(content);
            ResMap map = serializer.Deserialize(reader) as ResMap;
            m_AssetInfoInfoDic.Clear();
            if (map != null)
            {
                foreach (var info in map.m_AssetInfoList)
                {
                    m_AssetInfoInfoDic[info.m_AssetName] = info;
                }
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
            if (!m_AssetInfoInfoDic.TryGetValue(assetName, out AssetInfo info))
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