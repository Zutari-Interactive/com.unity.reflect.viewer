using System;
using System.Collections.Generic;
using UnityEngine;
using Zutari.LifeCycle;

namespace Zutari.Database
{
    [Serializable]
    public class LifeCycleDictionary : SDictionary<int, LifeCycleState>
    {
    }
}
