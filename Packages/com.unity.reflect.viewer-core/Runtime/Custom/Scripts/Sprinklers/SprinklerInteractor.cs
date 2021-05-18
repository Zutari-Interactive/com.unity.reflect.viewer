using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SprinklerInteractor : MonoBehaviour
{
    public GameObject maxPrefab;
    public GameObject minPrefab;

    private SprinklerManager manager;

    private void Start()
    {
        manager = FindObjectOfType<SprinklerManager>();
        if(manager == null)
        {
            Debug.LogWarning("no sprinkler manager active in scene");
        }

        Addressables.LoadAssetAsync<GameObject>("Sprinkler_Max").Completed += OnAddressableLoadDone;
    }

    private void OnAddressableLoadDone(AsyncOperationHandle<GameObject> obj)
    {
        if (obj.Result != null)
            maxPrefab = obj.Result;
        else
            Debug.LogWarning("No addressable asset found");
    }

    private void OnMouseDown()
    {
        DisplaySprinklerCoverage();
    }

    private void DisplaySprinklerCoverage()
    {
        foreach (var item in manager.ids)
        {
            Debug.Log("find sprinkler coverage");
            SprinklerGroup group = manager.dict[item] as SprinklerGroup;
            Vector3 tForm = group.pos;
            Vector3 spawnPos = FindPoint(tForm);
            if(spawnPos == Vector3.zero)
            {
                Debug.LogWarning($"no point found for {item}");
            }
            else
            {
                PlaceMinCoverage(spawnPos);
                PlaceMaxCoverage(spawnPos);
            }
        }
    }

    private Vector3 FindPoint(Vector3 shootFrom)
    {
        Vector3 hitPos = new Vector3(0,0,0);
        if (Physics.Raycast(shootFrom, Vector3.down, out RaycastHit hit, 10f))
        {
            hitPos = hit.point;
        }

        return hitPos;
    }

    private void PlaceMinCoverage(Vector3 pos)
    {
        GameObject coverageMin = Instantiate(minPrefab);
        coverageMin.transform.position = new Vector3(pos.x, (pos.y + 0.05f), pos.z);
    }

    private void PlaceMaxCoverage(Vector3 pos)
    {
        GameObject coverageMin = Instantiate(maxPrefab);
        coverageMin.transform.position = new Vector3(pos.x, (pos.y + 0.03f), pos.z);
    }
}
