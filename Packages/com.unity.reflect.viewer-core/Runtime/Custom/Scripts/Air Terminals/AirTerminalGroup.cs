using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTerminalGroup : CategoryGroup
{
    //public float maxCoverage;
    public string maxCoverage;
    public Vector3 pos;

    public override void AddObjectToList(Object newAirTerminal)
    {
        base.AddObjectToList(newAirTerminal);
    }

    public override void SaveGroupID(string id)
    {
        base.SaveGroupID(id);
    }
}
