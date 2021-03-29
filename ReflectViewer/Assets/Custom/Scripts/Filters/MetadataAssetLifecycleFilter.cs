using System.Collections;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

public class MetadataAssetLifecycleFilter : CustomFilter
{
    AssetLifecycleFilter m_AssetLifeCycleFilterProcessor;
    AssetLifecycleGrouper grouper;

    public override void AssignPipeline(PipelineAsset p)
    {
        base.AssignPipeline(p);
    }

    public override void SetupNode(Transform r)
    {
        // Create the node required
        if (pipelineAsset.TryGetNode<AssetLifecycleNode>(out AssetLifecycleNode assetLifecycleNode))
        {
            StartCoroutine(SetupFilter(grouper, assetLifecycleNode));
            return;
        }


        var filterNode = pipelineAsset.CreateNode<AssetLifecycleNode>();

        pipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        pipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        pipelineAsset.CreateConnection(syncNode.output, filterNode.instanceInput);
        pipelineAsset.CreateConnection(instanceNode.output, filterNode.gameObjectInput);

        // Once the pipeline is started, keep a link to the processor node so we can control filtering from it

        m_AssetLifeCycleFilterProcessor = filterNode.processor;
        SetupFilter(grouper, filterNode);
    }

    public override void SetupGrouper(Grouper g)
    {
        grouper = g as AssetLifecycleGrouper;
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
