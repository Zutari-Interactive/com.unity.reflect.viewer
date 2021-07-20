using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DataPackInteractor : MonoBehaviour
{
    public DataPack dp;

    private DataPackOptionsDialogController dataPackdialogController;
    private IOTSensorGroupDisplay sensorGroupDisplay;

    private string address;
    private IOTSensorGroup sensorGroup;
    private bool active;

    public bool Active
    {
        get => active;
        set { active = value; }
    }

    public void GetDataPack()
    {
        Addressables.LoadAssetAsync<DataPack>(address).Completed += OnAddressableLoadDone;

        dataPackdialogController = FindObjectOfType<DataPackOptionsDialogController>();
        sensorGroupDisplay = FindObjectOfType<IOTSensorGroupDisplay>();
    }

    private void OnAddressableLoadDone(AsyncOperationHandle<DataPack> obj)
    {
        if (obj.Result != null)
        {
            dp = obj.Result;
            //dp.sensorGroup = sensorGroup;
            sensorGroup.SaveIDs(dp.sensorIDs);
        }   
        else
            Debug.LogWarning("No addressable asset found");
    }

    

    public void SetAddress(string a)
    {
        address = a;
    }

    public void PrimeController()
    {
        if (Active == false)
        {
            Active = true;
            sensorGroup.SetupSensors(dp.iotSensorDisplayPrefab);
            dataPackdialogController.SetDataPack(dp);
            sensorGroupDisplay.SetSensorGroup(sensorGroup);
        }
        
    }

    public void CloseController()
    {
        sensorGroup.Close();
        Active = false;
    }

    public void SetSensorGroup(IOTSensorGroup g)
    {
        sensorGroup = g;
    }

}
