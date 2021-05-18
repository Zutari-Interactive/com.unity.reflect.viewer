using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Elements.General.Processes
{
    public static class ExternalProcesses
    {
        public static string CroissantProcess(string inPath, out string outPath)
        {
            outPath = string.Empty;
            if (string.IsNullOrEmpty(inPath))
            {
                Debug.LogWarning("Invalid Input Path to CSV File!");
                return default;
            }

            string filename = Path.GetFileNameWithoutExtension(inPath);

            // ToDo : To Avoid the FileNotFound Exception we create the File at the base Directory.
            outPath = $"{filename}.json";
            string command = $"{inPath} {outPath}";

            try
            {
                Process myProcess = new Process
                {
                    StartInfo =
                    {
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = true,
                        UseShellExecute = true,
                        FileName = Constants.Croissant
                    }
                };

                myProcess.StartInfo.Arguments = command;
                myProcess.EnableRaisingEvents = true;
                myProcess.Start();
                myProcess.WaitForExit();
                myProcess.Close();

                if (File.Exists(outPath)) Debug.Log("File Exists");

                // ToDo :  Here we move the File to Streaming Assets Folder for easy Access Later. Theoretically the File can be Cached in Application.persistentDataPath or Assets/ directory.
                string destination = Path.Combine(Application.streamingAssetsPath, $"{filename}.json");
                Debug.Log(destination);
                if (File.Exists(destination))
                {
                    File.Delete(destination);
                }

                if (File.Exists(outPath))
                {
                    File.Move(outPath, destination);
                }

                outPath = destination;

#if UNITY_EDITOR
                UnityEditor.AssetDatabase.Refresh();
#endif

                return File.ReadAllText(outPath);
            }
            catch (Exception exception)
            {
                Debug.Log(exception);
                return default;
            }
        }
    }
}
