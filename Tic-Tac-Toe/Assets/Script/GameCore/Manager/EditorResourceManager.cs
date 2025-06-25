#if UNITY_EDITOR
using System.Threading.Tasks;
using Script.Common;
using UnityEditor;
using UnityEngine;

namespace Script.GameCore
{
    public class EditorResourceManager : Singleton<EditorResourceManager>,IResourceManager
    {
        public T Load<T>(string path) where T : Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }

        public async Task<T> LoadAsync<T>(string path) where T : Object
        {
            return await Task.Run(() => Load<T>(path));
        }

        public void UnLoad(string path)
        {
            return;
        }

        public void UnLoadAll()
        {
            return;
        }
    }
}
#endif