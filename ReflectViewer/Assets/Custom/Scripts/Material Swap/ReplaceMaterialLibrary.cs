using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zutari.Database
{
    [CreateAssetMenu(fileName = "ReplaceMaterialLibrary", menuName = "Zutari/Databases/ReplaceMaterialLibrary")]
    public class ReplaceMaterialLibrary : SODatabase<string, Material>
    {

        #region VARIABLES

        [Header("Database")]
        public MatDict Database = new MatDict();

        #endregion
        
        #region METHODS

        public override void Add(string key, Material value)
        {
            Database.Add(key, value);
        }

        public override Material Get(string key)
        {
            return Database[key];
        }

        public override void UpdateValue(string key, Material value)
        {
            Database[key] = value;
        }

        public override bool Remove(string key)
        {
            return Database.Remove(key);
        }

        public override void Clear()
        {
            Database.Clear();
        }

        public override bool ContainsKey(string key)
        {
            return Database.ContainsKey(key);
        }

        #endregion

        public Material FetchMaterial(string matName)
        {
            Database.TryGetValue(matName, out Material value);
            return value;
        }

        public List<string> FetchKeyWords()
        {
            List<string> kw = new List<string>();
            foreach (var item in Database.Keys)
            {
                kw.Add(item);
            }
            return kw;
        }
    }
    
    [Serializable]
    public class MatDict : SDictionary<string, Material>
    {
    }
}
