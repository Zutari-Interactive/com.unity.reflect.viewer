using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;
using UnityEngine.Reflect.Pipeline;

[CreateAssetMenu(fileName = "Custom Filter Library", menuName = "Zutari Elements/Filter Library")]
public class CustomFilterLibrary : ScriptableObject
{
    [Header("Custom Reflect Nodes")]
    [Tooltip("Add all custom filters you want to use in this viewer here")]
    public CustomFilter[] customFilterPrefabs;

    [Header("Project Pipeline Asset")]
    public PipelineAsset pipeline;

    public void Setup(Transform r)
    {
        Debug.Log("run custom filter setup");
        if(customFilterPrefabs.Length == 0)
        {
            Debug.LogError("There are no custom filters active in this project.");
            return;
        }

        foreach (var item in customFilterPrefabs)
        {
            GameObject newFilter = Instantiate(item.gameObject);
            CustomFilter filter = newFilter.GetComponent<CustomFilter>();
            filter.AssignPipeline(pipeline);

            CustomNode n = filter.GetComponent<CustomNode>();

            //Grouper g = filter.GetComponent<Grouper>();
            if (n != null)
            {
                //filter.SetupGrouper(g);
                filter.SetupNode(n);
            }

            filter.SetupNode(r);
        }
    }
}
