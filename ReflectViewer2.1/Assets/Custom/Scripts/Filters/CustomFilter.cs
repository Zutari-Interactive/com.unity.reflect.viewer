using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

public class CustomFilter : MonoBehaviour
{
    #region VARIABLES

    private PipelineAsset _pipelineAsset;

    #endregion

    #region PROPERTIES

    public PipelineAsset PipelineAsset
    {
        get => _pipelineAsset;
        set => _pipelineAsset = value;
    }

    #endregion

    #region METHODS

    public virtual void AssignPipeline(PipelineAsset pipelineAsset)
    {
        PipelineAsset = pipelineAsset;
    }

    /// <summary>
    /// Used for sorting families in the hierarchy - mostly useful in the editor
    /// </summary>
    /// <param name="root"></param>
    public virtual void SetupNode(Transform root)
    {
    }

    /// <summary>
    /// Used all other custom nodes
    /// </summary>
    /// <param name="node"></param>
    public virtual void SetupNode(CustomNode node)
    {
    }

    /// <summary>
    /// Used for Grouping Common Components for interaction at runtime
    /// </summary>
    /// <param name="grouper"></param>
    public virtual void SetupGrouper(Grouper grouper)
    {
    }

    #endregion
}
