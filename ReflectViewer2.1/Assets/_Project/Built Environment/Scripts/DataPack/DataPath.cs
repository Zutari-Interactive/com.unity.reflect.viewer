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
#if PLATFORM_STANDALONE_WIN
        if (filePath != "")
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
#endif

#if UNITY_ANDROID

           string androidPath = "jar:file://" + appPath;
           var path = string.Format("{0}/{1}", androidPath, filePath);
           WWW wwwfile = new WWW(path);
           while (!wwwfile.isDone) { }
           File.WriteAllBytes(path, wwwfile.bytes);

           StreamReader wr = new StreamReader(filePath);
               string line;
               while ((line = wr.ReadLine()) != null)
               {
               //your code
               }



#endif
    }

    public void ShowFilePath()
    {
        UnityEngine.Debug.Log($"path assigned to this button is {appPath + filePath}");
    }
}
