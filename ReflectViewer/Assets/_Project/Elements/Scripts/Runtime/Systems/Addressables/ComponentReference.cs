using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Elements.Systems
{
    /// <summary>
    /// Custom Asset Reference Class that can be Derived From to Load/Instantiate Components of a GameObject
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    public class ComponentReference<TComponent> : AssetReference
    {
        public ComponentReference(string guid) : base(guid)
        {
        }

        public new AsyncOperationHandle<TComponent> InstantiateAsync(Vector3 position, Quaternion rotation,
                                                                     Transform parent = null)
        {
            return Addressables.ResourceManager
                               .CreateChainOperation(base.InstantiateAsync(position, Quaternion.identity, parent),
                                                     GameObjectReady);
        }

        public new AsyncOperationHandle<TComponent> InstantiateAsync(Transform parent = null,
                                                                     bool instantiateInWorldSpace = false)
        {
            return Addressables.ResourceManager
                               .CreateChainOperation(base.InstantiateAsync(parent, instantiateInWorldSpace),
                                                     GameObjectReady);
        }

        public AsyncOperationHandle<TComponent> LoadAssetAsync()
        {
            return Addressables.ResourceManager
                               .CreateChainOperation(base.LoadAssetAsync<GameObject>(),
                                                     GameObjectReady);
        }

        AsyncOperationHandle<TComponent> GameObjectReady(AsyncOperationHandle<GameObject> arg)
        {
            TComponent comp = arg.Result.GetComponent<TComponent>();
            return Addressables.ResourceManager.CreateCompletedOperation(comp, string.Empty);
        }

        public override bool ValidateAsset(Object obj)
        {
            GameObject go = obj as GameObject;
            return go != null && go.GetComponent<TComponent>() != null;
        }

        public override bool ValidateAsset(string path)
        {
#if UNITY_EDITOR
            //this load can be expensive...
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return go != null && go.GetComponent<TComponent>() != null;
#else
            return false;
#endif
        }

        public void ReleaseInstance(AsyncOperationHandle<TComponent> op)
        {
            // Release the instance
            Component component = op.Result as Component;
            if (component != null)
            {
                Addressables.ReleaseInstance(component.gameObject);
            }

            // Release the handle
            Addressables.Release(op);
        }
    }
}
