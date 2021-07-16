using UnityEngine;
using UnityEngine.Reflect;
using Unity.Reflect.Viewer.UI;
using UnityEngine.EventSystems;
using game4automation;
using System.Collections.Generic;

public class DataPackManager : CustomNode
{
    public string[] Ids;
    public GameObject iotSensorDisplayPrefab;

    private OPCUA_Interface opcInterface;

    private Canvas c;
    private bool _buttonEnabled;
    private Dictionary<string, IOTSensorGroup> sensorGroups = new Dictionary<string, IOTSensorGroup>();

    UISelectionController selectionController;

    public void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("DataPack");
        c = obj.GetComponent<Canvas>();

        OrphanUIController.onPointerClick += OnObjectClick;

        selectionController = FindObjectOfType<UISelectionController>();

        if(opcInterface == null)
        {
            opcInterface = GetComponent<OPCUA_Interface>();
            opcInterface.Connect();
        }
        
    }

    public void FindID(Metadata data)
        {
            foreach (var item in Ids)
            {
                string id = data.GetParameter("Mark");
                if (item.Equals(id))
                {
                    CreateDataPack(data.gameObject, id);
                    //create sensor group
                    //add sensor display object at position
                }
            }
        }

    public void OnObjectClick(BaseEventData data)
    {
        if(selectionController.m_CurrentSelectedGameObject != null)
        {
            if (selectionController.m_CurrentSelectedGameObject.GetComponent<DataPackInteractor>())
            {
                Debug.Log("mouse clicked");
                DataPackInteractor interactor = selectionController.m_CurrentSelectedGameObject.GetComponent<DataPackInteractor>();
                interactor.PrimeController();
                ButtonEnabled(true);
            }
            else
            {
                ButtonEnabled(false);
            }

        }
        else
        {
            ButtonEnabled(false);
        }
        
    }

    private void ButtonEnabled(bool v)
    {
        _buttonEnabled = v;
        c.enabled = v;
    }

    private void CreateDataPack(GameObject g, string id)
    {
        Debug.Log("data object found");
        if (!sensorGroups.ContainsKey(id))
        {
            DataPackInteractor dp = g.AddComponent(typeof(DataPackInteractor)) as DataPackInteractor;
            dp.SetAddress(id);
            dp.GetDataPack();
            SetupSensorGroup(g.transform, id);

        }
        else
        {
            sensorGroups[id].AddSensor(SetupSensor(sensorGroups[id], g.transform, id));
        }
        
    }

    private void SetupSensorGroup(Transform t, string id)
    {
        IOTSensorGroup sensorGroup = new IOTSensorGroup();
        sensorGroup.SetGroupName(id);
        sensorGroups.Add(id, sensorGroup);
        sensorGroup.AddSensor(SetupSensor(sensorGroup, t, id));
    }

    private IOTSensor SetupSensor(IOTSensorGroup sg, Transform t, string id)
    {
        GameObject obj = Instantiate(iotSensorDisplayPrefab);
        obj.transform.position = t.position;
        IOTSensor sensor = obj.GetComponent<IOTSensor>();
        sensor.SetupNode(id);
        return sensor;
    }
}

