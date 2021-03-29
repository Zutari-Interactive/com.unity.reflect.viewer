using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

public class MetadataLightingFilter : CustomFilter
{
    LightingFilter m_LightingFilterProcessor;
    LightGrouper grouper;

    public override void AssignPipeline(PipelineAsset p)
    {
        base.AssignPipeline(p);
    }

    public override void SetupNode(Transform r)
    {
        // Create the node required but first check whether it exists already
        if (pipelineAsset.TryGetNode<LightingNode>(out LightingNode lightingNode))
        {
            StartCoroutine(SetupFilter(grouper, lightingNode));
            return;
        }
            

        var filterNode = pipelineAsset.CreateNode<LightingNode>();

        pipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        pipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        pipelineAsset.CreateConnection(syncNode.output, filterNode.instanceInput);
        pipelineAsset.CreateConnection(instanceNode.output, filterNode.gameObjectInput);

        // Once the pipeline is started, keep a link to the processor node so we can control filtering from it

        m_LightingFilterProcessor = filterNode.processor;
    }

    public override void SetupGrouper(Grouper g)
    {
        grouper = g as LightGrouper;
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

