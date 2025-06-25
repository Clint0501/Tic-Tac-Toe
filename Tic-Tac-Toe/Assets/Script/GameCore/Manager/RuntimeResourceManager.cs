using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Script.Common;
using UnityEngine;

namespace Script.GameCore
{
    public class RuntimeResourceManager:Singleton<RuntimeResourceManager>,IResourceManager
    {
        private readonly string bundleRootPath;
        private AssetBundleManifest manifest;
        private readonly Dictionary<string, AssetBundle> loadedBundles = new Dictionary<string, AssetBundle>();
        private readonly Dictionary<string, Object> loadedAssets = new Dictionary<string, Object>();
        
        public RuntimeResourceManager(string bundleRoot)
        {
            bundleRootPath = bundleRoot;
            LoadManifest();
        }

        public RuntimeResourceManager()
        {
            
        }

        private void LoadManifest()
        {
            string manifestBundlePath = Path.Combine(bundleRootPath, "AssetBundles");
            var manifestBundle = AssetBundle.LoadFromFile(manifestBundlePath);
            if (manifestBundle != null)
            {
                manifest = manifestBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            }
            else
            {
                Debug.LogError("AssetBundleManifest 加载失败");
            }
        }
        
        private void LoadDependencies(string bundleName)
        {
            if (manifest == null) return;
            var dependencies = manifest.GetAllDependencies(bundleName);
            foreach (var dep in dependencies)
            {
                if (!loadedBundles.ContainsKey(dep))
                {
                    var depPath = Path.Combine(bundleRootPath, dep);
                    var depBundle = AssetBundle.LoadFromFile(depPath);
                    if (depBundle != null)
                        loadedBundles[dep] = depBundle;
                    else
                        Debug.LogError($"依赖包加载失败: {depPath}");
                }
            }
        }
        
        private async Task LoadDependenciesAsync(string bundleName)
        {
            if (manifest == null) return;
            var dependencies = manifest.GetAllDependencies(bundleName);
            foreach (var dep in dependencies)
            {
                if (!loadedBundles.ContainsKey(dep))
                {
                    var depPath = Path.Combine(bundleRootPath, dep);
                    var request = AssetBundle.LoadFromFileAsync(depPath);
                    await Task.Yield();
                    while (!request.isDone) await Task.Yield();
                    if (request.assetBundle != null)
                        loadedBundles[dep] = request.assetBundle;
                    else
                        Debug.LogError($"依赖包加载失败: {depPath}");
                }
            }
        }
        
        public T Load<T>(string path) where T : Object
        {
            string bundleName = GetBundleNameFromPath(path);
            LoadDependencies(bundleName);

            if (!loadedBundles.ContainsKey(bundleName))
            {
                string bundlePath = Path.Combine(bundleRootPath, bundleName);
                var bundle = AssetBundle.LoadFromFile(bundlePath);
                if (bundle != null)
                    loadedBundles[bundleName] = bundle;
                else
                    throw new FileNotFoundException($"AssetBundle 加载失败: {bundlePath}");
            }

            var asset = loadedBundles[bundleName].LoadAsset<T>(Path.GetFileName(path));
            loadedAssets[path] = asset;
            return asset;
        }

        public async Task<T> LoadAsync<T>(string path) where T : Object
        {
            string bundleName = GetBundleNameFromPath(path);
            await LoadDependenciesAsync(bundleName);

            if (!loadedBundles.ContainsKey(bundleName))
            {
                string bundlePath = Path.Combine(bundleRootPath, bundleName);
                var request = AssetBundle.LoadFromFileAsync(bundlePath);
                await Task.Yield();
                while (!request.isDone) await Task.Yield();
                if (request.assetBundle != null)
                    loadedBundles[bundleName] = request.assetBundle;
                else
                    throw new FileNotFoundException($"AssetBundle 加载失败: {bundlePath}");
            }

            var assetRequest = loadedBundles[bundleName].LoadAssetAsync<T>(Path.GetFileName(path));
            await Task.Yield();
            while (!assetRequest.isDone) await Task.Yield();
            loadedAssets[path] = assetRequest.asset;
            return assetRequest.asset as T;
        }

        public void UnLoad(string path)
        {
            if (loadedAssets.TryGetValue(path, out var asset))
            {
                Resources.UnloadAsset(asset);
                loadedAssets.Remove(path);
            }
        }

        public void UnLoadAll()
        {
            foreach (var asset in loadedAssets.Values)
            {
                Resources.UnloadAsset(asset);
            }
            loadedAssets.Clear();

            foreach (var bundle in loadedBundles.Values)
            {
                bundle.Unload(false);
            }
            loadedBundles.Clear();
        }
        
        
        // 工具方法：根据资源路径获取bundle名（需和打包规则一致）
        private string GetBundleNameFromPath(string path)
        {
            // 假设 path 形如 "Assets/Bundles/xxx/yyy.prefab"
            // bundleName 形如 "xxx"
            var fileName = Path.GetFileName(path);
            var dir = Path.GetDirectoryName(path);
            // 这里需根据你的实际打包规则调整
            return dir?.Split(Path.DirectorySeparatorChar)[^1].ToLower();
        }
    }
}