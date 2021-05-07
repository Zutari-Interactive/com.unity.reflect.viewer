using System;
using SerializableDictionary;

namespace Elements.LifeCycle
{
    [Serializable]
    public class AssetLifeCycleDictionary : SDictionary<int, AssetLifeCycleState>
    {
    }
}
