using System.Collections;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

public class MetadataAssetLifecycleFilter : CustomFilter
{
    AssetLifecycleFilter m_AssetLifeCycleFilterProcessor;
    AssetLifecycleGrouper grouper;

    public override void AssignPipeline(PipelineAsset pipelineAsset)
    {
        base.AssignPipeline(pipelineAsset);
    }

    public override void SetupNode(Transform r)
    {
        // Create the node required
        if (PipelineAsset.TryGetNode<AssetLifecycleNode>(out AssetLifecycleNode assetLifecycleNode))
        {
            StartCoroutine(SetupFilter(grouper, assetLifecycleNode));
            return;
        }


        var filterNode = PipelineAsset.CreateNode<AssetLifecycleNode>();

        PipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        PipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        PipelineAsset.CreateConnection(syncNode.output, filterNode.instanceInput);
        PipelineAsset.CreateConnection(instanceNode.output, filterNode.gameObjectInput);

        // Once the pipeline is started, keep a link to the processor node so we can control filtering from it

        m_AssetLifeCycleFilterProcessor = filterNode.processor;
        SetupFilter(grouper, filterNode);
    }

    public override void SetupGrouper(Grouper grouper)
    {
        this.grouper = grouper as AssetLifecycleGrouper;
    }

    public override void SetupNode(CustomNode node)
    {
        grouper = node as AssetLifecycleGrouper;
    }

    IEnumerator SetupFilter(AssetLifecycleGrouper grouper, AssetLifecycleNode node)
    {
        Debug.Log("wait for filter initialize");
        yield return new WaitForSecondsRealtime(2f);

        m_AssetLifeCycleFilterProcessor = node.processor;

        if (m_AssetLifeCycleFilterProcessor != null)
        {
            Debug.Log("run Asset Life Cycle Filter setup");
            m_AssetLifeCycleFilterProcessor.SetupGrouper(grouper);
        }
        else
        {
            Debug.Log("no filter yet assigned");
        }
        yield return null;
    }
}
