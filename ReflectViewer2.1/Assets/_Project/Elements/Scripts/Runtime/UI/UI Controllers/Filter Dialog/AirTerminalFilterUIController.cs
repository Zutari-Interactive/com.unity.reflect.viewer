using SharpFlux;
using SharpFlux.Dispatching;
using Unity.Reflect.Viewer.UI;
using Unity.TouchFramework;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.UI.Controllers
{
    [RequireComponent(typeof(DialogWindow))]
    public class AirTerminalFilterUIController : MonoBehaviour
    {
        #region VARIABLES

#pragma warning disable 649
        [SerializeField]
        Button m_DialogButton;

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
        }

        #endregion

        #region METHODS

        void OnDialogButtonClicked()
        {
            DialogType dialogType = m_DialogWindow.open ? DialogType.None : DialogType.AirTerminalFilter;
            Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));
        }

        void OnStateDataChanged(UIStateData data)
        {
            m_DialogButtonImage.enabled = data.activeDialog == DialogType.AirTerminalFilter;
            m_DialogButton.interactable = data.toolbarsEnabled;
        }

        #endregion
    }
}
