using game4automation;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// IOTSensor creates an OPC_Node that reads from the OPCNode object created by the OPCUA_Interface object
/// It then reads the incoming data of that OPCNode for display in the scene
/// </summary>
public class IOTSensor : MonoBehaviour
{
    [Header("Values from Sensor")]
    public TextMeshProUGUI sensorValueText;
    public TextMeshProUGUI sensorNameText;
    public DataTypes dataType;

    [Header("Value Offset")]
    [Tooltip("this value is used to modify the incoming sensor value to read naturally (instead of 101234 degress celcius, on 10.1 degrees celcius will show)")]
    public int valueOffset;

    public bool update;
    public bool canWrite;

    public string nodeID;
    //public Int16 sensorValue;

    public string SensorName { get; private set; }
    public string SensorValue { get; private set; }


    private OPCUA_Node node;
    private OPCUANode watchedNode;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (update)
        {
            //sensorValue = Int16.Parse(node.Value);
            node.UpdateNode();
            UpdateValue();
        }
    }

    public void AssignDataType(int i)
    {
        dataType = (DataTypes)i;
    }

    private void UpdateValue()
    {
        switch (dataType)
        {
            case DataTypes.Bool:
                BooleanUpdate();
                return;
            case DataTypes.Percentage:
                GeneralUpdate(" %");
                return;
            case DataTypes.Temperature:
                GeneralUpdate(" °C");
                return;
            case DataTypes.None:
                return;
        }
        sensorValueText.text = node.Value;
    }

    public void SetupNode(string nodeID)
    {
        FindNode(nodeID);
    }

    private void FindNode(string nodeID)
    {
        OPCUA_Node[] nodes = FindObjectsOfType<OPCUA_Node>();
        if (nodes.Length < 1)
        {
            Debug.LogWarning("no nodes found, exiting");
            return;
        }

        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i].NodeId.Equals(nodeID))
            {
                Debug.Log("node found");
                node = nodes[i];
                watchedNode = node.Interface.AddWatchedNode(nodeID);
                //sensorValue = Int16.Parse(node.Value);
                SensorName = ExtractName(node.Name);
                sensorNameText.text = SensorName;
                sensorValueText.text = node.Value;
                update = true;
                return;
            }
        }
    }

    private string ExtractName(string s)
    {
        var split = s.Split('.');
        return split[3];
    }

    private void BooleanUpdate()
    {
        if (node.Value.Equals("0"))
        {
            sensorValueText.text = "Off";
        }
        else if (node.Value.Equals("1"))
        {
            sensorValueText.text = "On";
        }
        else
        {
            //VNR = 'value not recognised'
            sensorValueText.text = "VNR";
        }
    }

    private void GeneralUpdate(string suffix)
    {
        var offsettValue = OffsetCalculation(node.Value);
        SensorValue = offsettValue.ToString("F") + suffix;
        sensorValueText.text = SensorValue;
    }

    private float OffsetCalculation(string value)
    {
        float fValue = float.Parse(value);
        float newValue = fValue / valueOffset;
        return newValue;
    }
}
