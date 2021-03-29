using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect.Pipeline;

public class MetadataMaterialFilter : CustomFilter
{
    MaterialFilter m_MaterialFilterProcessor;
    SwapMaterial swapper;

    public override void AssignPipeline(PipelineAsset p)
    {
        base.AssignPipeline(p);
    }

    public override void SetupNode(Transform r)
    {
        // Create the node required
        if (pipelineAsset.TryGetNode<MaterialNode>(out MaterialNode airTerminalNode))
        {
            StartCoroutine(SetupFilter(swapper, airTerminalNode));
            return;
        }


        var filterNode = pipelineAsset.CreateNode<MaterialNode>();

        pipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        pipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        pipelineAsset.CreateConnection(syncNode.output, filterNode.instanceInput);
        pipelineAsset.CreateConnection(instanceNode.output, filterNode.gameObjectInput);

        // Once the pipeline is started, keep a link to the processor node so we can control filtering from it

        m_MaterialFilterProcessor = filterNode.processor;
        SetupFilter(swapper, filterNode);
    }

    public override void SetupNode(CustomNode node)
    {
        swapper = node as SwapMaterial;
    }

    IEnumerator SetupFilter(SwapMaterial swap, MaterialNode node)
    {
        Debug.Log("wait for filter initialize");
        yield return new WaitForSecondsRealtime(2f);

        m_MaterialFilterProcessor = node.processor;

        if (m_MaterialFilterProcessor != null)
        {
            Debug.Log("run setup");
            m_MaterialFilterProcessor.Setup(swap);
        }
        else
        {
            Debug.Log("no filter yet assigned");
        }
        yield return null;
    }
}
