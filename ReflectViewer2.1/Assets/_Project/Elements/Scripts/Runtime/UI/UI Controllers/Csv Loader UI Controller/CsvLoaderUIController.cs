using Elements.General;
using Elements.General.IO;
using Elements.General.Processes;
using SharpFlux;
using SharpFlux.Dispatching;
using Unity.Reflect.Viewer.UI;
using Unity.TouchFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.UI.Controllers
{
    [RequireComponent(typeof(DialogWindow))]
    public class CsvLoaderUIController : MonoBehaviour
    {
        #region VARIABLES

#pragma warning disable 649
        [SerializeField]
        Button m_DialogButton;
        [SerializeField]
        Button m_LoadCsvButton;

#pragma warning restore 649

        DialogWindow m_DialogWindow;
        Image m_DialogButtonImage;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            UIStateManager.stateChanged += OnStateDataChanged;

            m_DialogButtonImage = m_DialogButton.GetComponent<Image>();
            m_DialogWindow = GetComponent<DialogWindow>();
        }

        void Start()
        {
            m_DialogButton.onClick.AddListener(OnDialogButtonClicked);
            m_LoadCsvButton.onClick.AddListener(OnLoadCsvButtonChanged);
        }

        #endregion

        #region METHODS

        void OnDialogButtonClicked()
        {
            DialogType dialogType = m_DialogWindow.open ? DialogType.None : DialogType.CsvLoader;
            Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));
        }

        void OnLoadCsvButtonChanged()
        {
            string csvPath = FileBrowser.OpenFilePanel("Select CSV", "", "csv", false);
            if (string.IsNullOrEmpty(csvPath)) return;
            string data = ExternalProcesses.CroissantProcess(csvPath, out Constants.JsonPath);
        }

        void OnStateDataChanged(UIStateData data)
        {
            m_DialogButtonImage.enabled = data.activeDialog == DialogType.CsvLoader;
            m_DialogButton.interactable = data.toolbarsEnabled;
        }

        #endregion
    }
}
