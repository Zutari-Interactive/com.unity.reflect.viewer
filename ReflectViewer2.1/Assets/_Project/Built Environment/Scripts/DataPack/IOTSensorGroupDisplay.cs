using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IOTSensorGroupDisplay : MonoBehaviour
{
    public TextMeshProUGUI groupNameText;
    public List<TextMeshProUGUI> sensorNames = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> sensorValues = new List<TextMeshProUGUI>();
    

    public void SetupGroupUI(IOTSensorGroup group)
    {
        groupNameText.text = group.groupName;

        for (int i = 0; i < group.sensorGroup.Count; i++)
        {
            if(sensorNames[i].gameObject.activeInHierarchy == false)
            {
                sensorNames[i].gameObject.SetActive(true);
            }

            sensorNames[i].text = group.sensorGroup[i].SensorName;

            if (sensorValues[i].gameObject.activeInHierarchy == false)
            {
                sensorValues[i].gameObject.SetActive(true);
            }
            
            sensorValues[i].text = group.sensorGroup[i].SensorValue;
        }

        for (int i = sensorNames.Count; i == group.sensorGroup.Count; i--)
        {
            sensorNames[i].gameObject.SetActive(false);
            sensorValues[i].gameObject.SetActive(false);
        }
    }
}
