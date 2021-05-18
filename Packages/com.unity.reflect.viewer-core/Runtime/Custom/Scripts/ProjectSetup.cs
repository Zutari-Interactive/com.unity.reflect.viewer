using Grpc.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;

[CreateAssetMenu(fileName = "Project Tools", menuName = "Zutari Elements/Project Tools")]
public class ProjectSetup : ScriptableObject
{
    public List<GameObject> GroupPrefabs = new List<GameObject>();

    public Dictionary<string, Grouper> groupsDict = new Dictionary<string, Grouper>();

    
    public void Init(TestOpenModel openModel)
    {
        foreach (var item in GroupPrefabs)
        {
            
            GameObject newTool = Instantiate(item);
            Grouper g = newTool.GetComponent<Grouper>();
            groupsDict.Add(item.name, g);
        }

        openModel.DataFilter += Filter;
    }

    private void Filter(UnityEngine.Reflect.Metadata data)
    {
        foreach (var item in groupsDict)
        {
            item.Value.FindDeviceIDs(data);
        }
    }
}
