using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

public class CustomFilter : MonoBehaviour
{
    [HideInInspector]
    public PipelineAsset pipelineAsset;

    public virtual void AssignPipeline(PipelineAsset p)
    {
        pipelineAsset = p;
    }

    //used for sorting families in the hierarchy - mostly useful in the editor
    public virtual void SetupNode(Transform root)
    {
        //currently no shared functionality between inheritors
    }

    //used all other custom nodes
    public virtual void SetupNode(CustomNode node)
    {
        //currently no shared functionality between inheritors
    }

    //used for grouping common components for interaction at runtime
    public virtual void SetupGrouper(Grouper g)
    {
        //currently no shared functionality between inheritors
    }
}
