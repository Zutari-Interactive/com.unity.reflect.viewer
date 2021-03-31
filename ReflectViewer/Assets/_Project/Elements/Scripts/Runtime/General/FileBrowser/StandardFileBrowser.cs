using SFB;

namespace Zutari.General
{
    public static class StandardFileBrowser
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
