using SFB;
using System.Collections;
using UnityEngine;

namespace Elements.General.IO
{
    public static class FileBrowser
    {
        private static string _path;

        #region METHODS

        public static string OpenFilePanel(string title = "", string directory = "", string extension = "",
                                             bool multiSelect = false)
        {
            return StandaloneFileBrowser.OpenFilePanel(title, directory, extension, multiSelect)?[0];
        }

        public static string SaveFilePanel(string title = "", string directory = "", string defaultName = "", string extension = "")
        {
            return StandaloneFileBrowser.SaveFilePanel(title, directory, defaultName, extension);
        }

        public static string[] OpenFolderPanel(string dataPath)
        {
            return StandaloneFileBrowser.OpenFolderPanel("Select Folder", dataPath, true);
        }

        public static void WriteResult(string path)
        {
            _path = path;
        }

        #endregion
    }
}
