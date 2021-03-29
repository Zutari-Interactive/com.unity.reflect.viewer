using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;

//script searches through all objects with given Parameter, groups all matching IDs together
[ExecuteInEditMode]
[RequireComponent(typeof(AirTerminalManager))]
public class AirTerminalGrouper : Grouper
{

    private void OnEnable()
    {
        manager = GetComponent<AirTerminalManager>();
        groups.Clear();
        manager.dict.Clear();
        //FindDeviceIDs();
    }

    private void OnValidate()
    {
        if (group)
        {
            groups.Clear();
            manager.dict.Clear();
            //FindDeviceIDs();
        }
    }

    public override void FindDeviceIDs(Metadata data)
    {
        var max = data.GetParameter(SearchParameter);

        if(max == "")
        {
            Debug.Log("no coverage value found");
        }
        else
        {
            var id = data.GetParameter("Id");
            var pos = data.gameObject.transform.position;
            CreateGroup(id, max, pos);
        }

        group = false;
    }

    private void CreateGroup(string id, string max, Vector3 objectPos)
    {
        AirTerminalGroup group = new AirTerminalGroup
        {
            groupID = id,
            //maxCoverage = float.Parse(max),
            maxCoverage = max,
            pos = objectPos
        };
        groups.Add(group);
        AddToManager(group);
    }

    private void AddToManager(AirTerminalGroup group)
    {
        manager.dict.Add(group.groupID, group);
        manager.ids.Add(group.groupID);
    }
}
