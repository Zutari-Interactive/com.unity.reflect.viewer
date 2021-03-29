using System;
using UnityEngine;
using Zutari.LifeCycle;

namespace Zutari.Database
{
    [CreateAssetMenu(fileName = "AssetStateMaterialDatabase", menuName = "Zutari/Databases/AssetStateMaterialDatabase")]
    public class AssetStateMaterialDatabase : SODatabase<AssetState, Material>
    {

        #region VARIABLES

        [Header("Database")]
        public AssetStateMaterialDictionary Database = new AssetStateMaterialDictionary();

        #endregion

        #region METHODS

        public override void Add(AssetState key, Material value)
        {
            Database.Add(key, value);
        }

        public override Material Get(AssetState key)
        {
            return Database[key];
        }

        public override void UpdateValue(AssetState key, Material value)
        {
            Database[key] = value;
        }

        public override bool Remove(AssetState key)
        {
            return Database.Remove(key);
        }

        public override void Clear()
        {
            Database.Clear();
        }

        public override bool ContainsKey(AssetState key)
        {
            return Database.ContainsKey(key);
        }

        #endregion
    }

    [Serializable]
    public class AssetStateMaterialDictionary : SDictionary<AssetState, Material>
    {
    }
}
