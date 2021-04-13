using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class DataPath : MonoBehaviour
{
    public string filePath;
    private string appPath;

    void Awake()
    {
        appPath = Application.dataPath;
        UnityEngine.Debug.Log("app path = " + appPath);
    }

    public void AssignPath(string p)
    {
        filePath = p;
    }

    public void OpenFile()
    {
        if(filePath != "")
        {
            ShowFilePath();
            Process myProcess = new Process();
            try
            {
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = appPath + filePath;
                myProcess.StartInfo.CreateNoWindow = false;
                myProcess.Start();
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e.Message);
            }
        }
    }

    public void ShowFilePath()
    {
        UnityEngine.Debug.Log($"path assigned to this button is {appPath + filePath}");
    }
}
