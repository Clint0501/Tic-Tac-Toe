using System.Threading.Tasks;
using UnityEngine;

namespace Script.GameCore
{
    public interface IResourceManager
    {
        T Load<T>(string path) where T : Object;
        Task<T> LoadAsync<T>(string path) where T : Object;
        void UnLoad(string path);
        void UnLoadAll();
    }
}