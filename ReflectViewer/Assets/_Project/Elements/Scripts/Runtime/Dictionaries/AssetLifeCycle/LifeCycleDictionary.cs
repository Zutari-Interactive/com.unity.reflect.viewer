using System;
using SerializableDictionary;
using Zutari.LifeCycle;

namespace Zutari.Database
{
    [Serializable]
    public class LifeCycleDictionary : SDictionary<int, LifeCycleState>
    {
    }
}
