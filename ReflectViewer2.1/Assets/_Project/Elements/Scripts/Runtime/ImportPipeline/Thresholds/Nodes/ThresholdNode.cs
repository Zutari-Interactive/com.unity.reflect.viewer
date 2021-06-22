using System;
using System.Collections.Generic;
using Unity.Reflect;
using Unity.Reflect.Model;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

namespace Zutari.Elements.Nodes
{
    #region REFLECT NODE

    [Serializable]
    public class ThresholdNode : ReflectNode<ThresholdFilter>
    {
        public StreamInstanceInput InstanceInput   = new StreamInstanceInput();
        public GameObjectInput     GameObjectInput = new GameObjectInput();

        protected override ThresholdFilter Create(ReflectBootstrapper hook, ISyncModelProvider provider,
                                                      IExposedPropertyTable resolver)
        {
            Debug.Log("Created new ThresholdFitler.");
            ThresholdFilter ThresholdFitler = new ThresholdFilter();

            InstanceInput.streamEvent = ThresholdFitler.OnStreamInstanceEvent;
            GameObjectInput.streamEvent = ThresholdFitler.OnGameObjectEvent;

            return ThresholdFitler;
        }

        #region VARIABLES

        // THIS IS FOR YOUR VARIABLES

        #endregion

        #region CUSTOM METHODS

        // THIS IS WHERE YOU ADD YOUR CUSTOM FILTER METHODS

        #endregion
    }

    #endregion

    #region IREFLECT NODE PROCESSOR FILTER

    public class ThresholdFilter : IReflectNodeProcessor
    {
        #region INTERNAL CLASS

        class FilterData
        {
            public bool                Visible   = true;
            public HashSet<GameObject> Instances = new HashSet<GameObject>();
        }

        #endregion

        #region VARIABLES

        private Dictionary<string, FilterData> _instances = new Dictionary<string, FilterData>();
        private ReflectClient _client;

        // ADD YOUR CUSTOM VARIABLES HERE
        // YOUR VARIABLES - START
        ThresholdManager thresholdManager;
        // YOUR VARIABLES - END

        #endregion

        #region PROPERTIES

        public IEnumerable<string> Categories
        {
            get { return _instances.Keys; }
        }

        #endregion

        #region CUSTOM METHODS

        // ADD YOUR CUSTOM METHODS HERE
        // YOUR METHODS - START

        public void Setup(ThresholdManager node)
        {
            thresholdManager = node;
        }

        public void SetupNode(CustomNode node)
        {
            Debug.Log("threshold node set up");
            thresholdManager = node as ThresholdManager;
        }

        // YOUR METHODS - END

        #endregion

        #region NODE PROCESSOR METHODS

        public void OnStreamEvent(SyncedData<GameObject> gameObject, StreamEvent streamEvent)
        {
            Debug.Log($"STREAM EVEN FIRED - {gameObject.data.name}");
        }

        public void OnStreamInstanceEvent(SyncedData<StreamInstance> stream, StreamEvent streamEvent)
        {
            if (streamEvent != StreamEvent.Added)
                return;

            SyncMetadata metadata = stream.data.instance.Metadata;

            if (metadata != null && metadata.Parameters.TryGetValue("Category", out SyncParameter category))
            {
                if (!_instances.ContainsKey(category.Value))
                {
                    _instances[category.Value] = new FilterData();
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

        private void OnGameObjectAdded(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out Metadata metadata))
                return;

            if (metadata.parameters.dictionary.TryGetValue("Category", out Metadata.Parameter category))
            {
                if (!_instances.TryGetValue(category.value, out FilterData filter))
                    return;


                filter.Instances.Add(gameObject);

                // YOUR CUSTOM CODE GOES HERE, THIS IS A UNIQUE CASE
                if (category.value.Equals("Doors"))
                {
                    // YOU CUSTOM CODE GOES HERE,
                    // ONCE A SUB CATEGORY HAS BEEN FOUND,
                    // CALL ANY CUSTOM FUNCTIONALITY THAT YOUR REQUIRE HERE

                    // YOUR CODE START
                    thresholdManager.Prepare(gameObject);
                    // YOUR CODE END
                }

                if (!filter.Visible)
                {
                    gameObject.SetActive(false);
                }
            }
        }

        private void OnGameObjectRemoved(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out Metadata metadata))
                return;

            if (metadata.parameters.dictionary.TryGetValue("Category", out Metadata.Parameter category))
            {
                if (_instances.TryGetValue(category.value, out FilterData filter))
                {
                    filter.Instances.Remove(gameObject);
                }
            }
        }

        public bool IsVisible(string category)
        {
            if (!_instances.TryGetValue(category, out FilterData filter))
                return true;

            return filter.Visible;
        }

        public void SetVisibility(string category, bool visible)
        {
            if (!_instances.TryGetValue(category, out FilterData filter))
                return;

            if (filter.Visible == visible)
                return;

            filter.Visible = visible;

            foreach (GameObject instance in filter.Instances)
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

        #endregion
    }
    #endregion
}
