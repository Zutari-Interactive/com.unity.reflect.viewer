using System.Collections;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

namespace Zutari.Elements.Nodes
{
    public class ViewFilterCreator : CustomFilter
    {
        #region VARIABLES

        private ViewFilter _nodeProcessorFilter;
        private ViewManager _manager;

        #endregion

        #region CUSTOM METHODS

        private IEnumerator SetupRoutine(ViewNode node, ViewManager m)
        {
            print($"{name} - Wait for Filter Initialization!");
            yield return new WaitForSecondsRealtime(2f);

            _nodeProcessorFilter = node.processor;

            if (_nodeProcessorFilter != null)
            {
                print($"{name} - Run Setup Process.");
                // YOUR CUSTOM CODE GOES HERE
                // YOUR CODE - START

                // Call your Filter Setup Method here
                _nodeProcessorFilter.Setup(m);

                // YOUR CODE - END
            }
            else
            {
                print($"{name} - No Filter Yet Assigned.");
                // YOUR CUSTOM CODE GOES HERE
                // YOUR CODE - START

                // YOUR CODE - END
            }

            yield return null;
        }

        #endregion


        #region OVERRIDE METHODS

        public override void AssignPipeline(PipelineAsset pipelineAsset)
        {
            base.AssignPipeline(pipelineAsset);
        }

        public override void SetupNode(Transform root)
        {
            if (PipelineAsset.TryGetNode(out ViewNode reflectNode))
            {
                // YOUR CUSTOM CODE GOES HERE
                // YOUR CODE - START

                // Edit the SetupRoutine Parameters if you require additional parameters.
                StartCoroutine(SetupRoutine(reflectNode, _manager));

                // YOUR CODE - END
                return;
            }


            ViewNode node = PipelineAsset.CreateNode<ViewNode>();

            PipelineAsset.TryGetNode(out SyncObjectInstanceProviderNode syncNode);
            PipelineAsset.TryGetNode(out InstanceConverterNode instanceNode);

            PipelineAsset.CreateConnection(syncNode.output, node.InstanceInput);
            PipelineAsset.CreateConnection(instanceNode.output, node.GameObjectInput);

            // Once the pipeline is started, keep a link to the processor node so we can control filtering from it
            _nodeProcessorFilter = node.processor;

            // YOUR CUSTOM CODE GOES HERE

            // YOUR CODE - START

            // Edit the SetupRoutine Parameters if you require additional parameters.
            StartCoroutine(SetupRoutine(reflectNode, _manager));

            // YOUR CODE - END
        }

        public override void SetupGrouper(Grouper grouper)
        {
            base.SetupGrouper(grouper);
        }

        public override void SetupNode(CustomNode node)
        {
            _manager = node as ViewManager;
        }

        #endregion
    }
}
