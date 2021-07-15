
using UnityEngine;
using UnityEngine.Reflect;
using Unity.Reflect.Viewer.UI;
using UnityEngine.EventSystems;
using game4automation;

public class DataPackManager : CustomNode
{
    public string[] Ids;

    private OPCUA_Interface opcInterface;

    private Canvas c;
    private bool _buttonEnabled;
    private DataPackOptions dpOptions;

    UISelectionController selectionController;

    public void Start()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("DataPack");
        c = obj.GetComponent<Canvas>();
        dpOptions = obj.GetComponentInChildren<DataPackOptions>();
        if(dpOptions == null)
        {
            Debug.LogError("no data pack options component found on button");
        }

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
                string id = data.GetParameter("Id");
                if (item.Equals(id))
                {
                    CreateDataPack(data.gameObject, id);
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
                dpOptions.PopulateOptions(interactor.dp);
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
        DataPackInteractor dp = g.AddComponent(typeof(DataPackInteractor)) as DataPackInteractor;
        dp.SetAddress(id);
        dp.GetDataPack();
    }
}

