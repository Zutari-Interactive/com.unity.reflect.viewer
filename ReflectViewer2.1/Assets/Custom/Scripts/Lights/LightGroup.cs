using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightGroup : CategoryGroup
{

    public override void AddObjectToList(Object newLight)
    {
        base.AddObjectToList(newLight);
    }

    public override void SaveGroupID(string id)
    {
        base.SaveGroupID(id);
    }
}
