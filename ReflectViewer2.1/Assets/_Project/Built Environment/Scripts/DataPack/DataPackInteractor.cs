using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DataPackInteractor : MonoBehaviour
{
    public DataPack dp;

    private DataPackOptionsDialogController dialogController;

    private string address;

    public void GetDataPack()
    {
        Debug.Log($"find pack with ID {address}");
        Addressables.LoadAssetAsync<DataPack>(address).Completed += OnAddressableLoadDone;

        dialogController = FindObjectOfType<DataPackOptionsDialogController>();
    }

    private void OnAddressableLoadDone(AsyncOperationHandle<DataPack> obj)
    {
        if (obj.Result != null)
            dp = obj.Result;
        else
            Debug.LogWarning("No addressable asset found");
    }

    public void SetAddress(string a)
    {
        address = a;
    }

    public void PrimeController()
    {
        dialogController.SetDataPack(dp);
        dialogController.SetSensorGroup(dp.sensorGroup);
    }

}
