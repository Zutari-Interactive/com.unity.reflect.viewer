using System.IO;
//using Newtonsoft.Json;
using UnityEngine;

namespace SerializableSO
{
    public enum SerializablePath
    {
        DataPath,
        PersistentPath,
        StreamingAssets,
    }

    public abstract class SerializableSOBase : ScriptableObject
    {
        #region VARIABLES

        [Header("Filename")]
        [HideInInspector]
        public string Filename = "variables.json";
        [HideInInspector]
        public SerializablePath SerializablePath = SerializablePath.PersistentPath;

        #endregion

        #region PROPERTIES

        #endregion

        #region SERIALIZATION METHODS

        public virtual string GetFilePath()
        {
            switch (SerializablePath)
            {
                case SerializablePath.DataPath:
                    return Path.Combine(Application.dataPath, Filename);
                case SerializablePath.PersistentPath:
                    return Path.Combine(Application.persistentDataPath, Filename);
                case SerializablePath.StreamingAssets:
                    if (!Directory.Exists(Application.streamingAssetsPath))
                        Directory.CreateDirectory(Application.streamingAssetsPath);
                    return Path.Combine(Application.streamingAssetsPath, Filename);
                default:
                    return Path.Combine(Application.persistentDataPath, Filename);
            }
        }

        public virtual void WriteToFile() => File.WriteAllText(GetFilePath(), JsonUtility.ToJson(this, true));

        public virtual void OverwriteFromFile()
        {
            if (!FileExist()) return;
            JsonUtility.FromJsonOverwrite(File.ReadAllText(GetFilePath()), this);
        }

        public virtual void DeleteFile()
        {
            if (!FileExist()) return;
            File.Delete(GetFilePath());
        }

        public virtual bool FileExist() => File.Exists(GetFilePath());

        #endregion
    }
}
