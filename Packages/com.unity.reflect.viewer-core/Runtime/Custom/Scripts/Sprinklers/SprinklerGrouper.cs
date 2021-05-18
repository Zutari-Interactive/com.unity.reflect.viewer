using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Reflect;

[ExecuteInEditMode]
[RequireComponent(typeof(SprinklerManager))]
public class SprinklerGrouper : Grouper
{

    private void OnEnable()
    {
        manager = GetComponent<Manager>();
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

    public override void AddInteractor(Metadata data)
    {
        var id = data.GetParameter("Category");
        if (id.Equals("Lighting Devices"))
        {
            if (!data.gameObject.GetComponent<SprinklerInteractor>())
                data.gameObject.AddComponent<SprinklerInteractor>();
        }
            
    }

    public override void FindDeviceIDs(Metadata data)
    {
        Debug.Log("find sprinkler ID");

        //initial ID check maybe not necessary, could maybe check directly for coverage values
        var id = data.GetParameter(SearchParameter);
        var max = data.GetParameter("Max Coverage");
        var min = data.GetParameter("Min Coverage");
        var pos = data.gameObject.transform.position;
        CreateGroup(id, max, min, pos);

        AddInteractor(data);
            
        group = false;
    }


    private void CreateGroup(string id, string max, string min, Vector3 objectPos)
    {
        Debug.Log("Creating sprinkler group with ID = " + id);
        SprinklerGroup group = new SprinklerGroup
        {
            groupID = id,
            maxCoverage = float.Parse(max),
            mincoverage = float.Parse(min),
            pos = objectPos
        };
        groups.Add(group);
        AddToManager(group);
    }

    private void AddToManager(SprinklerGroup group)
    {
        manager.dict.Add(group.groupID, group);
        manager.ids.Add(group.groupID);
    }
}

