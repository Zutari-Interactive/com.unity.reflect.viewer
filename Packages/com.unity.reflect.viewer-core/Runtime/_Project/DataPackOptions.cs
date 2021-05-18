using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Reflect.Viewer.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class DataPackOptions : MonoBehaviour
{
    public Canvas optionsCanvas;

    [SerializeField]
    private ToolButton _dataPackButton;

    private DataPath[] paths;

    void Start()
    {
        _dataPackButton = GetComponent<ToolButton>();
        _dataPackButton.buttonClicked += OpenCloseWindow;
        paths = optionsCanvas.gameObject.GetComponentsInChildren<DataPath>();
        if(paths.Length == 0)
        {
            Debug.LogError("No paths found");
        }
    }

    void OpenCloseWindow()
    {
        if (!optionsCanvas.enabled)
        {
            optionsCanvas.enabled = true;
        }
        else
        {
            optionsCanvas.enabled = false;
        }
    }

    public void PopulateOptions(DataPack pack)
    {
        for (int i = 0; i < pack.paths.Length; i++)
        {
            paths[i].AssignPath(pack.paths[i]);
            Debug.Log($"path assigned = {pack.paths[i]}");
        }
    }

    void OnDisable()
    {
        _dataPackButton.buttonClicked -= OpenCloseWindow;
    }
}
