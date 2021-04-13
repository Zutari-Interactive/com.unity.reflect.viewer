using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Zutari.Systems
{
    // Instantiate Async Creates AsyncOperationHandle Pointer
    // Use The AsyncOperationHandle to Release the Asset

    // When Instantiating Hundreds of Assets,
    // Use LoadAssetAsync, Save the Result (AsyncOperationHandle) and Call GameObject.Instantiate
    // using the AsyncOperationHandle.Result

    // Data Loading via
    // Addressables.LoadResourceLocationsAsync and
    // Addressables.GetDownloadSizeAsync
    // These functions Load data that can be accessed until the operation
    // is Released via Addressables.Release

    // Loading Scenes Release Unused Assets using Resources.

    public static class AddressableSystem
    {
        #region DOWNLOAD ASSET BUNDLE METHODS

        #endregion

        #region INSTANTIATE BY REFERENCE METHODS

        public static AsyncOperationHandle<GameObject> InstantiateByReference(AssetReference reference)
        {
            return reference.InstantiateAsync();
        }

        public static async Task<GameObject> InstantiateByReference(AssetReference reference, Transform parent = null)
        {
            return await reference.InstantiateAsync(parent).Task;
        }

        public static async Task<GameObject> InstantiateByReference(AssetReference reference, Vector3 position,
                                                                    Quaternion rotation,
                                                                    Transform parent = null)
        {
            return await reference.InstantiateAsync(position, rotation, parent).Task;
        }

        public static async Task<GameObject> InstantiateByReference(AssetReference reference,
                                                                    Action<AsyncOperationHandle<GameObject>> onLoad)
        {
            AsyncOperationHandle<GameObject> operationHandle = reference.InstantiateAsync();
            await operationHandle.Task;
            if (operationHandle.Status != AsyncOperationStatus.Succeeded) return null;
            operationHandle.Completed += onLoad;
            return operationHandle.Result;
        }

        #endregion

        #region INSTANTIATE BY LOCATION METHODS

        public static AsyncOperationHandle<GameObject> InstantiateByLocation(IResourceLocation location)
        {
            return Addressables.InstantiateAsync(location);
        }

        public static async Task<GameObject> InstantiateByLocation(IResourceLocation location, Transform parent = null)
        {
            return await Addressables.InstantiateAsync(location, parent).Task;
        }

        public static async Task<GameObject> InstantiateByLocation(IResourceLocation location, Vector3 position,
                                                                   Quaternion rotation, Transform parent = null)
        {
            return await Addressables.InstantiateAsync(location, position, rotation, parent).Task;
        }

        public static void InstantiateByLocation(IResourceLocation location,
                                                 Action<AsyncOperationHandle<GameObject>> onLoad)
        {
            Addressables.InstantiateAsync(location).Completed += onLoad;
        }

        #endregion

        #region INSTANTIATE BY PATH METHODS

        public static AsyncOperationHandle<GameObject> InstantiateByPath(string path)
        {
            return Addressables.InstantiateAsync(path);
        }

        public static async Task<GameObject> InstantiateByPath(string path, Transform parent = null)
        {
            return await Addressables.InstantiateAsync(path, parent).Task;
        }

        public static async Task<GameObject> InstantiateByPath(string path, Vector3 position, Quaternion rotation,
                                                               Transform parent = null)
        {
            return await Addressables.InstantiateAsync(path, position, rotation, parent).Task;
        }

        public static void InstantiateByPath(string path,
                                             Action<AsyncOperationHandle<GameObject>> onLoad)
        {
            Addressables.InstantiateAsync(path).Completed += onLoad;
        }

        #endregion

        #region INSTANTIATE FROM OPERATION HANDLE METHODS

        public static async Task<GameObject> InstantiateFromOperationHandle(
            AsyncOperationHandle<GameObject> operationHandle)
        {
            await operationHandle.Task;
            return operationHandle.Result == null || operationHandle.Status != AsyncOperationStatus.Succeeded
                ? null
                : Object.Instantiate(operationHandle.Result);
        }

        public static async Task<T> InstantiateFromOperationHandle<T>(
            AsyncOperationHandle<T> operationHandle) where T : Object
        {
            await operationHandle.Task;
            return Object.Instantiate(operationHandle.Result);
        }

        #endregion

        #region LOAD ASSET METHODS

        public static AsyncOperationHandle<T> LoadAssetByReference<T>(AssetReference reference)
        {
            return Addressables.LoadAssetAsync<T>(reference);
        }

        public static AsyncOperationHandle<T> LoadAssetAsync<T>(string location)
        {
            return Addressables.LoadAssetAsync<T>(location);
        }

        public static AsyncOperationHandle<T> LoadAssetAsync<T>(IResourceLocation location)
        {
            return Addressables.LoadAssetAsync<T>(location);
        }

        /// <summary>
        /// <para/>
        /// Load an Asset into Memory using the "LoadAssetAsync()" Method
        /// from a class Derived from the AssetReference Class.
        /// <para/>
        /// Note that this method uses the "dynamic" keyword to "assume" that the "reference" parameter
        /// is derived from the AssetReference Class.
        /// <para/>
        /// Note! This does not work with non Custom Asset Reference Classes.
        /// </summary>
        /// <param name="reference"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns>AsyncOperationHandle</returns>
        public static AsyncOperationHandle<T> LoadAssetAsync<T>(dynamic reference)
        {
            return reference.LoadAssetAsync();
        }

        #endregion

        #region LOAD AND CREATE METHODS

        #endregion

        #region LOAD RESOURCE LOCATIONS METHODS

        /// <summary>
        /// <para/>
        /// Load a List of IResourceLocations from Given Label.
        /// <para/>
        /// This handle needs to be Released when no Longer Needed!
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocations(string label)
        {
            return Addressables.LoadResourceLocationsAsync(label);
        }

        /// <summary>
        /// <para/>
        /// Load a List of IResourceLocations from Given List of Keys.
        /// <para/>
        /// This handle needs to be Released when no Longer Needed!
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public static AsyncOperationHandle<IList<IResourceLocation>> LoadResourceLocations(
            IEnumerable keys, Addressables.MergeMode mode)
        {
            return Addressables.LoadResourceLocationsAsync(keys, mode);
        }

        #endregion

        #region LOAD SCENE METHODS

        public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(
            AssetReference reference, LoadSceneMode mode = LoadSceneMode.Additive, bool activateOnLoad = true,
            int priority = 100)
        {
            return reference.LoadSceneAsync(mode, activateOnLoad, priority);
        }

        public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(
            IResourceLocation location, LoadSceneMode mode = LoadSceneMode.Additive,
            bool activateOnLoad = true, int priority = 100)
        {
            return Addressables.LoadSceneAsync(location, mode, activateOnLoad, priority);
        }

        public static AsyncOperationHandle<SceneInstance> LoadSceneAsync(
            string path, LoadSceneMode mode = LoadSceneMode.Additive,
            bool activateOnLoad = true, int priority = 100)
        {
            return Addressables.LoadSceneAsync(path, mode, activateOnLoad, priority);
        }

        public static async Task<AsyncOperationHandle<SceneInstance>> CustomLoadSceneAsync(
            AssetReference reference, LoadSceneMode mode = LoadSceneMode.Additive,
            bool activateOnLoad = true, int priority = 100,
            Action<AsyncOperationHandle<SceneInstance>> callback = null)
        {
            AsyncOperationHandle<SceneInstance> operationHandle =
                reference.LoadSceneAsync(mode, activateOnLoad, priority);
            await operationHandle.Task;

            operationHandle.Completed += callback;
            return operationHandle;
        }

        public static async Task UnloadSceneAsync(AsyncOperationHandle operationHandle)
        {
            await Addressables.UnloadSceneAsync(operationHandle).Task;
        }

        public static async Task UnloadSceneAsync(AsyncOperationHandle<SceneInstance> operationHandle)
        {
            await Addressables.UnloadSceneAsync(operationHandle).Task;
        }

        public static async Task CustomUnloadSceneAsync(AsyncOperationHandle operationHandle,
                                                        Action<AsyncOperationHandle<SceneInstance>> callback)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.UnloadSceneAsync(operationHandle);
            await handle.Task;

            handle.Completed += callback;
        }

        #endregion

        #region RELEASE ASSET METHODS

        public static bool ReleaseObject(GameObject reference)
        {
            return Addressables.ReleaseInstance(reference);
        }

        public static bool ReleaseAsyncOperation(AsyncOperationHandle handle)
        {
            return Addressables.ReleaseInstance(handle);
        }

        public static bool ReleaseAsyncOperation<T>(AsyncOperationHandle<T> handle) where T : Object
        {
            return Addressables.ReleaseInstance(handle);
        }

        #endregion
    }
}
