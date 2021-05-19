using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Reflect;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ViewManager : CustomNode
{

    private Vector3 viewPos;
    private Quaternion viewRot;

    public GameObject viewPrefab;
    private POI poi;

    private bool isOrthographic;


    void OnEnable()
    {
        Addressables.LoadAssetAsync<GameObject>("View").Completed += OnAddressableLoadDone;
    }

    public void CreateView(Metadata data)
    {
        poi = FetchPOIComponent(data.gameObject);
        if(poi == null)
        {
            Debug.LogError("no POI found on this view, aborting view creation");
            return;
        }
        viewPos = data.gameObject.transform.position;
        viewRot = data.gameObject.transform.rotation;

        SetupView();

        Debug.Log("View created");
    }

    private POI FetchPOIComponent(GameObject obj)
    {
        return obj.GetComponent<POI>();
    }

    public void SetupView()
    {
        GameObject newView = Instantiate(viewPrefab);

        Camera cam = newView.GetComponent<Camera>();
        cam.enabled = false;

        newView.transform.position = viewPos;
        newView.transform.rotation = viewRot;

        isOrthographic = poi.orthographic;

        if (isOrthographic)
        {
            cam.orthographic = isOrthographic;
        }


    }

    private void OnAddressableLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Result != null)
        {
            viewPrefab = obj.Result;
        }
        else
        {
            Debug.LogWarning("No addressable asset found for View");
        }
    }
}
