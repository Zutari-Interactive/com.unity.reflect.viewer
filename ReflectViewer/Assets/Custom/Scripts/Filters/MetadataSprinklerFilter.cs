using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

public class MetadataSprinklerFilter : CustomFilter
{
    SprinklerFilter m_SprinklerFilterProcessor;
    SprinklerGrouper grouper;

    public override void AssignPipeline(PipelineAsset pipelineAsset)
    {
        base.AssignPipeline(pipelineAsset);
    }

    public override void SetupNode(Transform r)
    {
        // Create the node required for this but first check if it exists
        if (PipelineAsset.TryGetNode<SprinklerNode>(out SprinklerNode sprinklerNode))
        {
            StartCoroutine(SetupFilter(grouper, sprinklerNode));
            return;
        }


        var filterNode = PipelineAsset.CreateNode<SprinklerNode>();

        PipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        PipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        PipelineAsset.CreateConnection(syncNode.output, filterNode.instanceInput);
        PipelineAsset.CreateConnection(instanceNode.output, filterNode.gameObjectInput);

        // Once the pipeline is started, keep a link to the processor node so we can control filtering from it

        m_SprinklerFilterProcessor = filterNode.processor;
    }

    public override void SetupGrouper(Grouper grouper)
    {
        this.grouper = grouper as SprinklerGrouper;
    }

    public override void SetupNode(CustomNode node)
    {
        grouper = node as SprinklerGrouper;
    }

    IEnumerator SetupFilter(SprinklerGrouper grouper, SprinklerNode node)
    {
        Debug.Log("wait for filter initialize");
        yield return new WaitForSecondsRealtime(2f);

        m_SprinklerFilterProcessor = node.processor;

        if (m_SprinklerFilterProcessor != null)
        {
            Debug.Log("run setup");
            m_SprinklerFilterProcessor.SetupGrouper(grouper);
        }
        else
        {
            Debug.Log("no filter yet assigned");
        }
        yield return null;
    }
}
