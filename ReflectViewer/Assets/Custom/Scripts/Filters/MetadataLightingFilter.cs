using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

public class MetadataLightingFilter : CustomFilter
{
    LightingFilter m_LightingFilterProcessor;
    LightGrouper grouper;

    public override void AssignPipeline(PipelineAsset pipelineAsset)
    {
        base.AssignPipeline(pipelineAsset);
    }

    public override void SetupNode(Transform r)
    {
        // Create the node required but first check whether it exists already
        if (PipelineAsset.TryGetNode<LightingNode>(out LightingNode lightingNode))
        {
            StartCoroutine(SetupFilter(grouper, lightingNode));
            return;
        }
            

        var filterNode = PipelineAsset.CreateNode<LightingNode>();

        PipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        PipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        PipelineAsset.CreateConnection(syncNode.output, filterNode.instanceInput);
        PipelineAsset.CreateConnection(instanceNode.output, filterNode.gameObjectInput);

        // Once the pipeline is started, keep a link to the processor node so we can control filtering from it

        m_LightingFilterProcessor = filterNode.processor;
    }

    public override void SetupGrouper(Grouper grouper)
    {
        this.grouper = grouper as LightGrouper;
    }

    public override void SetupNode(CustomNode node)
    {
        grouper = node as LightGrouper;
    }

    IEnumerator SetupFilter(LightGrouper grouper, LightingNode node)
    {
        Debug.Log("wait for filter initialize");
        yield return new WaitForSecondsRealtime(2f);

        m_LightingFilterProcessor = node.processor;

        if (m_LightingFilterProcessor != null)
        {
            Debug.Log("run setup");
            m_LightingFilterProcessor.SetupGrouper(grouper);
        }
        else
        {
            Debug.Log("no filter yet assigned");
        }
        yield return null;
    }
}

