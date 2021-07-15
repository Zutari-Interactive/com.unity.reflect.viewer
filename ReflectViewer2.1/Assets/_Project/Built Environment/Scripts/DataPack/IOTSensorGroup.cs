using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IOTSensorGroup
{
    public List<IOTSensor> sensorGroup = new List<IOTSensor>();
    public string groupName;

    public void SetGroupName(string name)
    {
        groupName = name;
    }

    public void AddSensor(IOTSensor s)
    {
        sensorGroup.Add(s);
    }
}
