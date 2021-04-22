using SFB;

namespace Elements.IO
{
    public static class FileBrowser
    {
        #region METHODS

        public static string OpenFilePanel(string title = "", string directory = "", string extension = "",
                                             bool multiSelect = false)
        {
            return StandaloneFileBrowser.OpenFilePanel(title, directory, extension, multiSelect)?[0];
        }

        #endregion
    }
}
