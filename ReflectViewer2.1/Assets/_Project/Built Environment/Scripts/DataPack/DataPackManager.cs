using UnityEngine;
using UnityEngine.Reflect;
using Unity.Reflect.Viewer.UI;
using UnityEngine.EventSystems;
using game4automation;
using System.Collections.Generic;

public class DataPackManager : CustomNode
{
    public string[] Ids;
    //public GameObject iotSensorDisplayPrefab;

    private OPCUA_Interface opcInterface;

    private Canvas c;
    private bool _buttonEnabled;
    private Dictionary<string, IOTSensorGroup> sensorGroups = new Dictionary<string, IOTSensorGroup>();

    UISelectionController selectionController;

    private DataPackInteractor currentInteractor;

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

        sensorGroups.Clear();
    }

    public void FindID(Metadata data)
        {
            foreach (var item in Ids)
            {
                string id = data.GetParameter("Mark");
                if (item.Contains(id))
                {
                    Debug.Log($"Data Object Found {data.GetParameter("Id")} with Mark {id}");

                    CreateDataPack(data.gameObject, id);
                }
            }
        }

    public void OnObjectClick(BaseEventData data)
    {
        //if(currentInteractor != null)
        //{
        //    currentInteractor.CloseController();
        //}
        

        if(selectionController.m_CurrentSelectedGameObject != null)
        {
            if (selectionController.m_CurrentSelectedGameObject.GetComponent<DataPackInteractor>())
            {
                Debug.Log("mouse clicked");
                currentInteractor = selectionController.m_CurrentSelectedGameObject.GetComponent<DataPackInteractor>();
                currentInteractor.PrimeController();
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
        
        if (!sensorGroups.ContainsKey(id))
        {
            DataPackInteractor dp = g.AddComponent(typeof(DataPackInteractor)) as DataPackInteractor;
            dp.SetAddress(id);
            dp.GetDataPack();
            SetupSensorGroup(g.transform, id);
            dp.SetSensorGroup(sensorGroups[id]);
        }
        //else
        //{
        //    sensorGroups[id].AddSensor(SetupSensor(g.transform, id));
        //}

    }

    //maybe these should go in the interactor?
    private IOTSensorGroup SetupSensorGroup(Transform t, string id)
    {
        IOTSensorGroup sensorGroup = new IOTSensorGroup();
        sensorGroup.SetGroup(id, t);
        sensorGroups.Add(id, sensorGroup);
        //sensorGroup.AddSensor(SetupSensor(t, id));
        return sensorGroup;
    }

    //private IOTSensor SetupSensor(Transform t, string id)
    //{
    //    GameObject obj = Instantiate(iotSensorDisplayPrefab);
    //    obj.transform.position = t.position;
    //    IOTSensor sensor = obj.GetComponent<IOTSensor>();
    //    sensor.SetupNode(id);
    //    return sensor;
    //}
}

