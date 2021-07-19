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
            string nodeName = ExtractName(nodes[i].NodeId);
            if (nodeName.Equals(nodeID))
            {
                if(nodes[i].Name == "EU Units" || nodes[i].Name == "Item Time Zone")
                {
                    nodes[i].enabled = false;
                    continue;
                }
                Debug.Log($"node {nodes[i].Name} found");
                node = nodes[i];
                watchedNode = node.Interface.AddWatchedNode(nodeID);
                //sensorValue = Int16.Parse(node.Value);
                SensorName = nodeID;
                sensorNameText.text = SensorName;
                DetermineValueType();
                UpdateValue();
                return;
            }
        }
    }

    

    private string ExtractName(string s)
    {
        var split = s.Split('.');
        return split[split.Length - 1];
    }

    private void DetermineValueType()
    {
        Debug.Log("Sensor data type = " + node.Type);
        switch (node.Type)
        {
            case "UInt16":
                dataType = DataTypes.Temperature;
                break;
            case "Boolean":
                dataType = DataTypes.Bool;
                break;
        }
    }

    private void BooleanUpdate()
    {
        if (node.Value.Equals("False"))
        {
            sensorValueText.text = "Off";
            SensorValue = "Off";
        }
        else if (node.Value.Equals("True"))
        {
            sensorValueText.text = "On";
            SensorValue = "On";
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
