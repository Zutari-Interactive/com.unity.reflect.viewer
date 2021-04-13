using System.IO;
using UnityEngine;

namespace Variables
{
    public abstract class VariablesBase : ScriptableObject
    {
        #region VARIABLES

        [Header("Filename")]
        [HideInInspector]
        public string Filename = "variables.json";

        #endregion

        #region PROPERTIES

        public string FilePath => Path.Combine(Application.persistentDataPath, Filename);

        #endregion

        #region SERIALIZATION METHODS

        public virtual void WriteToFile() => File.WriteAllText(FilePath, JsonUtility.ToJson(this));

        public virtual void OverwriteFromFile()
        {
            if (!Exists()) return;
            JsonUtility.FromJsonOverwrite(File.ReadAllText(FilePath), this);
        }

        public virtual void DeleteFile()
        {
            if (!Exists()) return;
            File.Delete(FilePath);
        }

        public virtual bool Exists()
        {
            return File.Exists(FilePath);
        }

        #endregion
    }
}
