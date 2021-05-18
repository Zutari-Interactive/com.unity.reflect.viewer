using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect.Pipeline;

public class MetadataMaterialFilter : CustomFilter
{
    MaterialFilter m_MaterialFilterProcessor;
    SwapMaterial swapper;

    public override void AssignPipeline(PipelineAsset pipelineAsset)
    {
        base.AssignPipeline(pipelineAsset);
    }

    public override void SetupNode(Transform r)
    {
        // Create the node required
        if (PipelineAsset.TryGetNode<MaterialNode>(out MaterialNode airTerminalNode))
        {
            StartCoroutine(SetupFilter(swapper, airTerminalNode));
            return;
        }


        var filterNode = PipelineAsset.CreateNode<MaterialNode>();

        PipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        PipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        PipelineAsset.CreateConnection(syncNode.output, filterNode.instanceInput);
        PipelineAsset.CreateConnection(instanceNode.output, filterNode.gameObjectInput);

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
