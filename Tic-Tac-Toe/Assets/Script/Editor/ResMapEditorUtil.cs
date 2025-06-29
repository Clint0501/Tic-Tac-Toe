using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
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
            resMap.m_AssetInfoList.Clear();
            string[] resGuids = AssetDatabase.FindAssets("", s_AssetBundleResourcePaths);
            foreach (string guid in resGuids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                
                // 过滤文件夹
                if (AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(DefaultAsset) && AssetDatabase.IsValidFolder(path))
                {
                    continue;
                }
                
                AssetInfo assetInfo = new AssetInfo
                {
                    m_AssetPath = path,
                    m_BundleName = GetBundleName(path),
                    m_AssetName = Path.GetFileNameWithoutExtension(path),
                    m_Dependencies = GetDependenciesBundle(path)
                };
                if (assetInfo.m_BundleName != String.Empty)
                {
                    resMap.m_AssetInfoList.Add(assetInfo);
                    
                }
                else
                {
                    Debug.LogError($"{assetInfo.m_AssetName}未能获取正确的bundle包名");
                }
                
            }


            string dir = Path.GetDirectoryName(ResManager.s_ResMapXMLPath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            
            // 序列化为XML
            XmlSerializer serializer = new XmlSerializer(typeof(ResMap));
            using (FileStream stream = new FileStream(ResManager.s_ResMapXMLPath, FileMode.Create))
            {
                serializer.Serialize(stream, resMap);
            }
            Debug.Log("ResMap.xml 生成完毕：" + ResManager.s_ResMapXMLPath);
            AssetDatabase.Refresh();
        }

        private static List<string> GetDependenciesBundle(string path)
        {
            List<string> result = new List<string>();
            string[] dependencies = AssetDatabase.GetDependencies(path);
            foreach (string dependencyPath in dependencies)
            {
                bool isBundle = false;
                foreach (string bundleDir in s_AssetBundleResourcePaths)
                {
                    if (dependencyPath.Replace("\\","/").Contains(bundleDir))
                    {
                        isBundle = true;
                        break;

                    }
                }

                if (!isBundle) continue;
                string dependencyBundleName = GetBundleName(dependencyPath);
                if (dependencyBundleName != GetBundleName(path) && result.Contains(dependencyBundleName))
                {
                    result.Add(dependencyBundleName);
                }
                    
            }

            return result;
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
        
        
        public static void BuildAssetBundlesFromResMap(ResMap resMap, string outputPath)
        {
            // 1. 按BundleName分组
            Dictionary<string, List<string>> bundleDict = new Dictionary<string, List<string>>();
            foreach (AssetInfo info in resMap.m_AssetInfoList)
            {
                if (!bundleDict.ContainsKey(info.m_BundleName))
                    bundleDict[info.m_BundleName] = new List<string>();
                bundleDict[info.m_BundleName].Add(info.m_AssetPath);
            }

            // 2. 生成AssetBundleBuild数组
            List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
            foreach (KeyValuePair<string, List<string>> kv in bundleDict)
            {
                AssetBundleBuild build = new AssetBundleBuild
                {
                    assetBundleName = kv.Key,
                    assetNames = kv.Value.ToArray()
                };
                buildList.Add(build);
            }

            // 3. 调用BuildPipeline
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            BuildPipeline.BuildAssetBundles(
                outputPath,
                buildList.ToArray(),
                BuildAssetBundleOptions.None,
                EditorUserBuildSettings.activeBuildTarget
            );

            UnityEngine.Debug.Log("AB包打包完成，输出目录：" + outputPath);
        }

        #endregion
       
    }
}