using System.IO;
using Script.GameCore;
using UnityEditor;
using UnityEngine;

namespace Script.Editor
{
    public static class ResMapEditorTool
    {
        [MenuItem("Bundle/自动化设置资源Bundle名称")]
        public static void SetBundleName()
        {
            // 获取所有资源
            var allAssets = AssetDatabase.FindAssets("", ResMapEditorUtil.s_AssetBundleResourcePaths);
        
            foreach (string guid in allAssets)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            
                // 跳过.meta文件
                if (Path.GetExtension(assetPath) == ".meta")
                    continue;
            
                // 根据文件夹设置Bundle名
                string bundleName = ResMapEditorUtil.GetBundleName(assetPath);
            
                // 设置Bundle名称
                var importer = AssetImporter.GetAtPath(assetPath);
                if (importer != null)
                {
                    importer.assetBundleName = bundleName;
                    Debug.Log($"设置Bundle名: {assetPath} -> {bundleName}");
                }
            }
        
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("Bundle名称设置完成！");
        }
        
        [MenuItem("Bundle/构建AB包（目前仅仅实现Windows平台）")]
        static void BuildAllAssetBundles()
        {
            // 确保输出目录存在
            if (!Directory.Exists(ResManager.s_LocalBundlesPath))
            {
                Directory.CreateDirectory(ResManager.s_LocalBundlesPath);
            }
        
            // 构建AssetBundles
            BuildPipeline.BuildAssetBundles(
                ResManager.s_LocalBundlesPath, // 输出目录
                BuildAssetBundleOptions.ChunkBasedCompression,  // 构建选项
                BuildTarget.StandaloneWindows  // 目标平台
            );
        
            Debug.Log("AssetBundles构建完成！");
        }
        
    }
}