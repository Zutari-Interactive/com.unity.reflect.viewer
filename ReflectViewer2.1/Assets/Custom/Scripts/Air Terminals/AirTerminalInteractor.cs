using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AirTerminalInteractor : Interactor
{ 
    public GameObject maxPrefab;

    private AirTerminalManager manager;

    private void Start()
    {
        manager = FindObjectOfType<AirTerminalManager>();
        if (manager == null)
        {
            Debug.LogWarning("no air terminal manager active in scene");
        }

        Addressables.LoadAssetAsync<GameObject>("Air_Flow").Completed += OnAddressableLoadDone;
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
        //DisplayDiffuserCoverage();
    }

    public void DisplayDiffuserCoverage()
    {
        StartCoroutine(ShowAirFlow());
    }

    private Vector3 FindPoint(Vector3 shootFrom)
    {
        Debug.Log("raycasting");
        Vector3 hitPos = new Vector3(0, 0, 0);
        if (Physics.Raycast(shootFrom, Vector3.down, out RaycastHit hit, 10f))
        {
            Debug.Log("hit" + hit.collider.name);
            hitPos = hit.point;
        }

        return hitPos;
    }

    private void PlaceMaxCoverage(Vector3 pos)
    { 
        GameObject coverageMin = Instantiate(maxPrefab);
        coverageMin.transform.position = new Vector3(pos.x, (pos.y + 0.03f), pos.z);
    }

    IEnumerator ShowAirFlow()
    {
        Debug.Log("calculating coverage");
        if (manager == null)
        {
            manager = FindObjectOfType<AirTerminalManager>();
        }

        foreach (var item in manager.ids)
        {
            AirTerminalGroup group = manager.dict[item] as AirTerminalGroup;
            PlaceMaxCoverage(group.pos);

            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void ToolAvailabilityCheck()
    {
        StartCoroutine(RunCheck());
    }

    IEnumerator RunCheck()
    {
        yield return new WaitForSeconds(5f);

        manager = FindObjectOfType<AirTerminalManager>();

        if (manager == null)
        {
            RectTransform cv = GetComponent<RectTransform>();
            cv.gameObject.SetActive(false);
        }
        
    }
}
