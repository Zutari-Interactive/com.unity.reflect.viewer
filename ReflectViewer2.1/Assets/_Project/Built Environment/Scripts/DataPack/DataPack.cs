using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DataPack", menuName = "Zutari Elements/External Data Holder")]
public class DataPack : ScriptableObject
{
    public string[] paths;

    public string[] sensorIDs;

    public GameObject iotSensorDisplayPrefab;

    public bool pathsAvailable;

    //public IOTSensorGroup sensorGroup;

    private void Awake()
    {
        //sensorGroup = new IOTSensorGroup();
        if(paths.Length > 0)
        {
            pathsAvailable = true;
        }
    }
}
