using System.Collections;
using System.Collections.Generic;
using game4automation;
using UnityEngine;

public class DemoReadNodeValueWithStatus : MonoBehaviour
{
    public OPCUA_Node node;
    public string Status;
    public string Servertimestamp;
    public bool IsGood;
    public bool IsBad;

    public string Value;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var val = node.ReadNodeValue();
        if (val != null)
          Value = node.ReadNodeValue().ToString();
        Status = node.Status;
        Servertimestamp = node.ServerTimestamp;
        IsGood = node.StatusGood;
        IsBad = node.StatusBad;
    }
}
