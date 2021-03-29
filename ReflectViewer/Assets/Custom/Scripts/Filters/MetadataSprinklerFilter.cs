using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

public class MetadataSprinklerFilter : CustomFilter
{
    SprinklerFilter m_SprinklerFilterProcessor;
    SprinklerGrouper grouper;

    public override void AssignPipeline(PipelineAsset p)
    {
        base.AssignPipeline(p);
    }

    public override void SetupNode(Transform r)
    {
        // Create the node required for this but first check if it exists
        if (pipelineAsset.TryGetNode<SprinklerNode>(out SprinklerNode sprinklerNode))
        {
            StartCoroutine(SetupFilter(grouper, sprinklerNode));
            return;
        }


        var filterNode = pipelineAsset.CreateNode<SprinklerNode>();

        pipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        pipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        pipelineAsset.CreateConnection(syncNode.output, filterNode.instanceInput);
        pipelineAsset.CreateConnection(instanceNode.output, filterNode.gameObjectInput);

        // Once the pipeline is started, keep a link to the processor node so we can control filtering from it

        m_SprinklerFilterProcessor = filterNode.processor;
    }

    public override void SetupGrouper(Grouper g)
    {
        grouper = g as SprinklerGrouper;
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
