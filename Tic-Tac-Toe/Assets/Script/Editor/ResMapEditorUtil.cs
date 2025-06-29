using System;
using System.Collections.Generic;
using System.IO;
using Script.GameCore;
using UnityEditor;
using UnityEngine;

namespace Script.Editor
{
    public class ResMapEditorUtil
    {
        public static readonly string[] s_AssetBundleResourcePaths = new []{"Assets/BundleResources/"};
        
        
        #region 资源映射表初始化

        [MenuItem("Bundle/生成资源映射表")]
        public static void GenerateResMap()
        {
            ResMap resMap = new ResMap();
            resMap.m_AssetInfoMap.Clear();
            string[] resGuids = AssetDatabase.FindAssets("", s_AssetBundleResourcePaths);
            foreach (string guid in resGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                AssetInfo assetInfo = new AssetInfo
                {
                    m_AssetPath = path,
                    m_BundleName = GetBundleName(path),
                    m_AssetName = Path.GetFileNameWithoutExtension(path)
                };
                if (assetInfo.m_BundleName != String.Empty)
                {
                    resMap.m_AssetInfoMap.Add(assetInfo.m_AssetName, assetInfo);
                }
                else
                {
                    Debug.LogError($"{assetInfo.m_AssetName}未能获取正确的bundle包名");
                }
                
            }

            //收集依赖
            foreach (KeyValuePair<string,AssetInfo> pair in resMap.m_AssetInfoMap)
            {
                AssetInfo assetInfo = pair.Value;
                assetInfo.m_Dependencies ??= new List<string>();
                assetInfo.m_Dependencies.Clear();
                string[] dependencies = AssetDatabase.GetDependencies(assetInfo.m_AssetPath);
                foreach (string dependencyPath in dependencies)
                {
                    if (!dependencyPath.Contains("BundleResources"))
                    {
                        continue;
                    }
                    string assetName = Path.GetFileNameWithoutExtension(dependencyPath);
                    if (resMap.m_AssetInfoMap.TryGetValue(assetName, out AssetInfo dependencyAssetInfo) 
                        && assetInfo.m_Dependencies.Contains(dependencyAssetInfo.m_BundleName))
                    {
                        assetInfo.m_Dependencies.Add(dependencyAssetInfo.m_BundleName);
                    }
                    
                }
            }
            
            //将资源映射表写到本地
            File.WriteAllText(ResManager.s_ResMapJsonPath, JsonUtility.ToJson(resMap));
        }
        public static string GetBundleName(string assetPath)
        {
            string dir = Path.GetDirectoryName(assetPath)?.Replace("\\", "/");
            if (dir != null)
            {
                string[] subDirs = dir.Split("/");
                string bundleName = String.Empty;
                for (int i = 0; i < subDirs.Length; i++)
                {
                    bundleName += subDirs[i];
                    if (i != subDirs.Length - 1)
                    {
                        bundleName += "_";
                    }
                }

                return bundleName;
            }
            return String.Empty;
        }

        #endregion
       
    }
}