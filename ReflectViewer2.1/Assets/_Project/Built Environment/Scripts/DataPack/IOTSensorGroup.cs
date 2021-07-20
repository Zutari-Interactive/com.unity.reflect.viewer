using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class IOTSensorGroup : MonoBehaviour
{
    public List<IOTSensor> sensorGroup = new List<IOTSensor>();
    public string groupName;

    private string[] ids = new string[0];
    private Transform groupPos;

    public void SetGroup(string name, Transform p)
    {
        groupName = name;
        groupPos = p;
    }

    public void AddSensor(IOTSensor s)
    {
        sensorGroup.Add(s);
        Debug.Log("Added sensor to Group " + groupName);
    }

    public void SaveIDs(string[] x)
    {
        ids = x;
    }

    public void SetupSensors(GameObject prefab)
    {
        for (int i = 0; i < ids.Length; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.transform.position = groupPos.position;
            IOTSensor sensor = obj.GetComponent<IOTSensor>();
            sensor.SetupNode(ids[i]);
            AddSensor(sensor);
        }
    }

    public void Close()
    {
        for (int i = 0; i < sensorGroup.Count; i++)
        {
            IOTSensor sensor = sensorGroup[i];
            sensorGroup.Remove(sensor);
            Destroy(sensor.gameObject);
        }
    }
}
