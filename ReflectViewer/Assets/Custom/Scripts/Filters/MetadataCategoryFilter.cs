using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect.Pipeline;

public class MetadataCategoryFilter : CustomFilter
{
    CategoryFilter m_CategoryFilterProcessor;

    public override void AssignPipeline(PipelineAsset p)
    {
        base.AssignPipeline(p);
    }

    public override void SetupNode(Transform r)
    {
        // Create the node required
        if (pipelineAsset.TryGetNode<CategoryNode>(out CategoryNode categoryNode))
        {
            StartCoroutine(SetupFilter(r, categoryNode));
            return;
        }
            

        var filterNode = pipelineAsset.CreateNode<CategoryNode>();

        pipelineAsset.TryGetNode<SyncObjectInstanceProviderNode>(out SyncObjectInstanceProviderNode syncNode);
        pipelineAsset.TryGetNode<InstanceConverterNode>(out InstanceConverterNode instanceNode);

        pipelineAsset.CreateConnection(syncNode.output, filterNode.instanceInput);
        pipelineAsset.CreateConnection(instanceNode.output, filterNode.gameObjectInput);

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
