using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

public class MetadataAirTerminalFilter : CustomFilter
{
    AirTerminalFilter m_AirTerminalFilterProcessor;
    AirTerminalGrouper grouper;

    public override void AssignPipeline(PipelineAsset p)
    {
        base.AssignPipeline(p);
    }

    public override void SetupNode(Transform r)
    {
        // Create the node required
        if (pipelineAsset.TryGetNode<AirTerminalNode>(out AirTerminalNode airTerminalNode))
        {
            StartCoroutine(SetupFilter(grouper, airTerminalNode));
            return;
        }


        var filterNode = pipelineAsset.CreateNode<AirTerminalNode>();

        pipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        pipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        pipelineAsset.CreateConnection(syncNode.output, filterNode.instanceInput);
        pipelineAsset.CreateConnection(instanceNode.output, filterNode.gameObjectInput);

        // Once the pipeline is started, keep a link to the processor node so we can control filtering from it

        m_AirTerminalFilterProcessor = filterNode.processor;
        SetupFilter(grouper, filterNode);
    }

    public override void SetupGrouper(Grouper g)
    {
        grouper = g as AirTerminalGrouper;
    }

    public override void SetupNode(CustomNode node)
    {
        grouper = node as AirTerminalGrouper;
    }

    IEnumerator SetupFilter(AirTerminalGrouper grouper, AirTerminalNode node)
    {
        Debug.Log("wait for filter initialize");
        yield return new WaitForSecondsRealtime(2f);

        m_AirTerminalFilterProcessor = node.processor;

        if (m_AirTerminalFilterProcessor != null)
        {
            Debug.Log("run setup");
            m_AirTerminalFilterProcessor.SetupGrouper(grouper);
        }
        else
        {
            Debug.Log("no filter yet assigned");
        }
        yield return null;
    }
}
