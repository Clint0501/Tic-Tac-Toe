using System;
using System.Collections.Generic;
using Script.Common;
using System.Xml.Serialization;

namespace Script.GameCore
{
    /// <summary>
    /// 资源信息
    /// </summary>
    [Serializable]
    public class AssetInfo
    {
        [XmlAttribute] public string m_AssetName;
        [XmlAttribute] public string m_AssetPath;
        [XmlAttribute] public string m_BundleName;
        [XmlArray("Dependencies"), XmlArrayItem("Bundle")]
        public List<string> m_Dependencies = new List<string>();
    }

    /// <summary>
    /// 资源映射表
    /// </summary>
    [Serializable]
    [XmlRoot("ResMap")]
    public class ResMap
    {
        [XmlArray("Assets"), XmlArrayItem("AssetInfo")]
        public List<AssetInfo> m_AssetInfoList = new List<AssetInfo>();
    }
}