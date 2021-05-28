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
    private StaticViewCollection viewCollection;

    private POI poi;

    private bool isOrthographic;
    private bool viewsPresent;


    void OnEnable()
    {
        viewCollection = GetComponent<StaticViewCollection>();
    }

    public void SetupView(Metadata data)
    {
        poi = FetchPOIComponent(data.gameObject);
        if (poi == null)
        {
            Debug.LogError("no POI found on this view, aborting view creation");
            return;
        }
        isOrthographic = poi.orthographic;
        viewCollection.CreateNewView(data.transform, isOrthographic);
        Debug.Log("created new view");
    }

    private POI FetchPOIComponent(GameObject obj)
    {
        return obj.GetComponent<POI>();
    }
}
