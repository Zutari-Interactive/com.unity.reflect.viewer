using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect.Pipeline;
using Zutari.Elements.Nodes;

public class DataPackFilterCreator : CustomFilter
{
    DataPackFilter m_FilterProcessor;

    public override void AssignPipeline(PipelineAsset p)
    {
        base.AssignPipeline(p);
    }

    public override void SetupNode(CustomNode n)
    {
        // Create the node required
        if (pipelineAsset.TryGetNode<DataPackNode>(out DataPackNode dataPackNode))
        {
            StartCoroutine(SetupFilter(n, dataPackNode));
            return;
        }

        var filterNode = pipelineAsset.CreateNode<DataPackNode>();

        pipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        pipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        pipelineAsset.CreateConnection(syncNode.output, filterNode.InstanceInput);
        pipelineAsset.CreateConnection(instanceNode.output, filterNode.GameObjectInput);

        // Once the pipeline is started, keep a link to the processor node so we can control filtering from it

        m_FilterProcessor = filterNode.processor;
        SetupFilter(n, filterNode);
    }

    IEnumerator SetupFilter(CustomNode n, DataPackNode node)
    {
        Debug.Log("wait for filter initialize");
        yield return new WaitForSecondsRealtime(2f);

        m_FilterProcessor = node.processor;

        if (m_FilterProcessor != null)
        {
            Debug.Log("run setup");
            m_FilterProcessor.Setup(n);
        }
        else
        {
            Debug.Log("no filter yet assigned");
        }
        yield return null;
    }
}
