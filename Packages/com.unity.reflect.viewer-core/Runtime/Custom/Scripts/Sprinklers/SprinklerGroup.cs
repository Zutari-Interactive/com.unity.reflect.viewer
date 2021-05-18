using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprinklerGroup : CategoryGroup
{
    public float maxCoverage;
    public float mincoverage;
    public Vector3 pos;

    public override void AddObjectToList(Object newSprinkler)
    {
        base.AddObjectToList(newSprinkler);
    }

    public override void SaveGroupID(string id)
    {
        base.SaveGroupID(id);
    }
}
