using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Script.GameCore;
using UnityEditor;
using UnityEngine;

namespace Script.Editor
{
    public static class ResMapEditorTool
    {
        [MenuItem("Bundle/构建AB包（目前仅仅实现Windows平台）")]
        public static void BuildAssetBundles()
        {
            string content = File.ReadAllText(ResManager.s_ResMapXMLPath);
            if (content == String.Empty)
            {
                Debug.LogError("AssetMap.xml 未找到！");
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(ResMap));
            using StringReader reader = new StringReader(content);
            ResMap map = serializer.Deserialize(reader) as ResMap;
            ResMapEditorUtil.BuildAssetBundlesFromResMap(map, ResManager.s_LocalBundlesPath);
        }
    }
}