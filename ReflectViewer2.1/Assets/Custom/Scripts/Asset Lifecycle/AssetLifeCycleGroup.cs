using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLifeCycleGroup : CategoryGroup
{
    public override void AddObjectToList(Object newAssetLifeCycle)
    {
        group.Add(newAssetLifeCycle as GameObject);
    }

    public override void SaveGroupID(string id)
    {
        base.SaveGroupID(id);
    }
}
