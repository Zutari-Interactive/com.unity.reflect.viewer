using System.Collections;
using System.Collections.Generic;
using Unity.Reflect;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

//Zutari Elements custom node

[SerializeField]
class MaterialNode : ReflectNode<MaterialFilter>
{
    public StreamInstanceInput instanceInput = new StreamInstanceInput();
    public GameObjectInput gameObjectInput = new GameObjectInput();
    protected override MaterialFilter Create(ReflectBootstrapper hook, ISyncModelProvider provider, IExposedPropertyTable resolver)
    {
        Debug.Log("created category filter");
        var filter = new MaterialFilter();

        instanceInput.streamEvent = filter.OnStreamInstanceEvent;
        gameObjectInput.streamEvent = filter.OnGameObjectEvent;

        return filter;
    }
}


class MaterialFilter : IReflectNodeProcessor
{
    class FilterData
    {
        public bool visible = true;
        public HashSet<GameObject> instances = new HashSet<GameObject>();
    }

    Dictionary<string, FilterData> m_Instances = new Dictionary<string, FilterData>();

    ReflectClient client;


    //custom
    private SwapMaterial swapper;

    public void Setup(SwapMaterial sm)
    {
        Debug.Log("setting up Material Filter");
        swapper = sm;
    }

    public void SetupNode(CustomNode node)
    {
        swapper = node as SwapMaterial;
    }
    //
    public IEnumerable<string> categories
    {
        get { return m_Instances.Keys; }
    }

    public void OnStreamEvent(SyncedData<GameObject> gameObject, StreamEvent streamEvent)
    {
        Debug.Log("stream event fired");
    }

    public void OnStreamInstanceEvent(SyncedData<StreamInstance> stream, StreamEvent streamEvent)
    {
        if (streamEvent != StreamEvent.Added)
            return;

        var metadata = stream.data.instance.Metadata;

        if (metadata != null && metadata.Parameters.TryGetValue("Category", out var category))
        {
            if (!m_Instances.ContainsKey(category.Value))
            {
                m_Instances[category.Value] = new FilterData();
            }
        }
    }

    public void OnGameObjectEvent(SyncedData<GameObject> gameObjectData, StreamEvent streamEvent)
    {
        if (streamEvent == StreamEvent.Added)
        {
            OnGameObjectAdded(gameObjectData.data);
        }
        else if (streamEvent == StreamEvent.Removed)
        {
            OnGameObjectRemoved(gameObjectData.data);
        }
    }

    void OnGameObjectAdded(GameObject gameObject)
    {
        if (!gameObject.TryGetComponent<Metadata>(out var metadata))
            return;

        if (metadata.parameters.dictionary.TryGetValue("Category", out var category))
        {
            if (!m_Instances.TryGetValue(category.value, out var filter))
                return;

            filter.instances.Add(gameObject);
            //custom
            swapper.SortObject(metadata);

            if (!filter.visible)
            {
                gameObject.SetActive(false);
            }
        }
    }

    void OnGameObjectRemoved(GameObject gameObject)
    {
        if (!gameObject.TryGetComponent<Metadata>(out var metadata))
            return;

        if (metadata.parameters.dictionary.TryGetValue("Category", out var category))
        {
            if (m_Instances.TryGetValue(category.value, out var filter))
            {
                filter.instances.Remove(gameObject);
            }
        }
    }

    public bool IsVisible(string category)
    {
        if (!m_Instances.TryGetValue(category, out var filter))
            return true;

        return filter.visible;
    }

    public void SetVisibility(string category, bool visible)
    {
        if (!m_Instances.TryGetValue(category, out var filter))
            return;

        if (filter.visible == visible)
            return;

        filter.visible = visible;

        foreach (var instance in filter.instances)
        {
            instance.SetActive(visible);
        }
    }

    public void OnPipelineInitialized()
    {
        // OnPipelineInitialized is called the first time the pipeline is run.
    }

    public void OnPipelineShutdown()
    {
        // OnPipelineShutdown is called before the pipeline graph is destroyed.
    }


}
