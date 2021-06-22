using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThresholdManager : CustomNode
{
    public void Prepare(GameObject obj)
    {
        obj.layer = 2;
    }
}
