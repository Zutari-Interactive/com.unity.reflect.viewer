using System.IO;
using Unity.TouchFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using Zutari.General;
using Zutari.General.External;

namespace Zutari.UI
{
    public class LoadCsvFile : MonoBehaviour, IPointerClickHandler
    {
        #region UNITY METHODS

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                OpenFileBrowser();
            }
        }

        #endregion

        #region METHODS

        public void OpenFileBrowser()
        {
            string csvPath = StandardFileBrowser.OpenFilePanel("Select CSV", "", "csv", false);
            if (string.IsNullOrEmpty(csvPath)) return;
            string data = ExternalProcesses.CroissantProcess(csvPath, out Constants.JsonPath);

            print("Processing File!");
            ProcessData.CallProcessData(data);
        }

        #endregion
    }
}
