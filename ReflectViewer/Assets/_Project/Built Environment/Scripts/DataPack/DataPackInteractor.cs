using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DataPackInteractor : MonoBehaviour
{
    public DataPack dp;

    private string address;

    private void GetDataPack()
    {
        Debug.Log($"find pack with ID {address}");
        Addressables.LoadAssetAsync<DataPack>(address).Completed += OnAddressableLoadDone;
    }

    private void OnAddressableLoadDone(AsyncOperationHandle<DataPack> obj)
    {
        if (obj.Result != null)
            dp = obj.Result;
        else
            Debug.LogWarning("No addressable asset found");
    }

    private void OnMouseDown()
    {
        GetDataPack();
    }

    public void SetAddress(string a)
    {
        address = a;
    }
}
