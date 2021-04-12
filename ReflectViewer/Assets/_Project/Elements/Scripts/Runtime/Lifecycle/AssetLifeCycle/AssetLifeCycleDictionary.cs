using System;
using SerializableDictionary;

namespace Zutari.LifeCycle
{
    [Serializable]
    public class AssetLifeCycleDictionary : SDictionary<int, AssetLifeCycleState>
    {
    }
}
