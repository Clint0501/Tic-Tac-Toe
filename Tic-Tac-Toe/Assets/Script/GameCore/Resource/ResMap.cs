using System;
using System.Collections.Generic;
using Script.Common;

namespace Script.GameCore
{
    /// <summary>
    /// 资源信息
    /// </summary>
    [Serializable]
    public class AssetInfo
    {
        public string m_AssetPath;
        public string m_BundleName;
        public string m_AssetName;
        public List<string> m_Dependencies;
    }

    /// <summary>
    /// 资源映射表
    /// </summary>
    public class ResMap
    {
        public Dictionary<string,AssetInfo> m_AssetInfoMap = new Dictionary<string,AssetInfo>();
    }
}