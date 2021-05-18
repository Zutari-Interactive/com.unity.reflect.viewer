using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect.Pipeline;

public class MetadataCategoryFilter : CustomFilter
{
    CategoryFilter m_CategoryFilterProcessor;

    public override void AssignPipeline(PipelineAsset pipelineAsset)
    {
        base.AssignPipeline(pipelineAsset);
    }

    public override void SetupNode(Transform r)
    {
        // Create the node required
        if (PipelineAsset.TryGetNode<CategoryNode>(out CategoryNode categoryNode))
        {
            StartCoroutine(SetupFilter(r, categoryNode));
            return;
        }
            

        var filterNode = PipelineAsset.CreateNode<CategoryNode>();

        PipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        PipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        PipelineAsset.CreateConnection(syncNode.output, filterNode.instanceInput);
        PipelineAsset.CreateConnection(instanceNode.output, filterNode.gameObjectInput);

        // Once the pipeline is started, keep a link to the processor node so we can control filtering from it

        m_CategoryFilterProcessor = filterNode.processor;
        SetupFilter(r, filterNode);
    }

    IEnumerator SetupFilter(Transform r, CategoryNode node)
    {
        Debug.Log("wait for filter initialize");
        yield return new WaitForSecondsRealtime(2f);

        m_CategoryFilterProcessor = node.processor;

        if (m_CategoryFilterProcessor != null)
        {
            Debug.Log("run setup");
            m_CategoryFilterProcessor.Setup(r);
        }
        else
        {
            Debug.Log("no filter yet assigned");
        }
        yield return null;
    }
}
