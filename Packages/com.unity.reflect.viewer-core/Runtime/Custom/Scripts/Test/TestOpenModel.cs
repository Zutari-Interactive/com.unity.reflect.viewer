using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Reflect.Model;
using UnityEditor;
using UnityEngine;
using UnityEngine.Reflect;

public class TestOpenModel : MonoBehaviour
{
    public Transform Root;

    private SyncPrefabBinding _syncPrefabBinding;
    private WaitUntil _waitUntil;

    //murray
    public ProjectSetup setup;
    private int syncObjectCount = 0;
    private int objectCounter = 0;
    SortCategories cs;
    public event Action<Metadata> DataFilter;

    private void Start()
    {
        FindSyncManager();
        setup.Init(this);
    }

    public void FindUnitySyncBinding()
    {
        StartCoroutine(FindSyncPrefabBinding());
    }

    private IEnumerator FindSyncPrefabBinding()
    {
        _waitUntil = new WaitUntil(() => Root.childCount > 0);
        while (!_syncPrefabBinding)
        {
            yield return _waitUntil;
            _syncPrefabBinding = Root.GetChild(0).GetChild(0).GetComponent<SyncPrefabBinding>();
            _syncPrefabBinding.gameObject.AddComponent<SortCategories>();
            print($"Sync Prefab Binding : {_syncPrefabBinding}");
        }

        print("Exit Find Sync Prefab Binding!");
    }

    private void BeginSetup()
    {
        GameObject go = _syncPrefabBinding.gameObject;
        cs = go.GetComponent<SortCategories>();
        if (cs != null)
        {
            cs.SearchParameter = "Category";
        }
    }

    private void FindSyncManager()
    {
        //SyncManager sm = FindObjectOfType<SyncManager>();
        //sm.onInstanceAdded += InstanceAdded;
    }

    //private void InstanceAdded(SyncInstance instance)
    //{
    //    instance.onObjectCreated += ObjectCreated;
    //}

    private void ObjectCreated(SyncObjectBinding obj)
    {
        if (objectCounter == 0)
        {
            BeginSetup();
            objectCounter++;
        }

        Debug.Log("Object created");
        Metadata data = obj.GetComponent<Metadata>();
        cs.SortObject(data);
        DataFilter?.Invoke(data);
    }

    public void StartSync()
    {
        throw new NotImplementedException();
    }

    public void StopSync()
    {
        throw new NotImplementedException();
    }
}
