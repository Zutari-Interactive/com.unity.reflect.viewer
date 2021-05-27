using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.TouchFramework;
using UnityEngine.Reflect;
using TMPro;
using System;
using SharpFlux.Dispatching;
using SharpFlux;
using UnityEngine.UI;
using Elements.General.IO;
using System.Threading.Tasks;

namespace Unity.Reflect.Viewer.UI
{

    [RequireComponent(typeof(DialogWindow))]

    public class ViewCameraDialogController : MonoBehaviour
    {
        
        public List<ToolButton> viewButtons = new List<ToolButton>();
        public ToolButton screenshotButton;
        public ToolButton returnButton;
        public SlideToggle hideUIToggle;
        public Canvas uiRootCanvas;

        private StaticViewCollection viewCollection;

        private bool hideUI;

        MinMaxPropertyControl fieldOfViewController;

        DialogWindow m_DialogWindow;
        StatsInfoData m_CurrentStatsInfoData;
        DialogType m_CachedActiveDialog;
        ToolState m_CurrentToolState;

        [SerializeField]
        Sprite m_InfoImage;
        [SerializeField]
        Sprite m_DebugImage;

        void Awake()
        {
            //UIStateManager.stateChanged += OnStateDataChanged;
            //UIStateManager.debugStateChanged += OnDebugStateDataChanged;
            //m_DialogWindow = GetComponent<DialogWindow>();
        }

        // Start is called before the first frame update
        void Start()
        {
            m_DialogWindow = GetComponent<DialogWindow>();
            fieldOfViewController = GetComponentInChildren<MinMaxPropertyControl>();
            screenshotButton.buttonClicked += TakeScreenshot;
            returnButton.buttonClicked += ReturnToMainCamera;
            hideUIToggle.onValueChanged.AddListener(ToggleUI);
            if (hideUIToggle.on)
            {
                ToggleUI(true);
            }
        }

        private void ReturnToMainCamera()
        {
            viewCollection.ReturnToMainCamera();
            OnViewButtonClicked();
        }

        private void OnViewButtonLongPressed()
        {
            Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, DialogType.ViewCameraDialog));
        }

        private void OnViewButtonClicked()
        {
            UIStateManager.stateChanged += OnStateDataChanged;
            UIStateManager.debugStateChanged += OnDebugStateDataChanged;

            if(viewCollection == null)
            {
                viewCollection = FindObjectOfType<StaticViewCollection>();
            }

            if (m_CurrentToolState.infoType == InfoType.Info)
            {
                var dialogType = m_DialogWindow.open ? DialogType.None : DialogType.ViewCameraDialog;
                Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));

                if (!m_DialogWindow.open)
                {
                    UIStateManager.stateChanged -= OnStateDataChanged;
                    UIStateManager.debugStateChanged -= OnDebugStateDataChanged;
                }
            }
            if (m_CurrentToolState.infoType == InfoType.Debug)
            {
                //var dialogType = m_DebugDialogWindow.open ? DialogType.None : DialogType.DebugOptions;
                //Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));
                Debug.LogWarning("no debug dialog available currently");
            }
        }

        public void AddNewButton(ToolButton b, int i)
        {
            b.buttonClicked += OnViewButtonClicked;
            b.buttonLongPressed += OnViewButtonLongPressed;
            viewButtons.Add(b);
            Debug.Log($"View Button {i} subscribed");
        }

        private void OnDebugStateDataChanged(UIDebugStateData data)
        {
            Debug.LogWarning("debug on this dialog not currently available");
        }

        private void OnStateDataChanged(UIStateData data)
        {
            if(viewButtons.Count == 0)
                return;

            foreach (ToolButton b in viewButtons)
            {
                Debug.Log("tool button state change");
                if (m_CachedActiveDialog != data.activeDialog)
                {
                    b.selected = (data.activeDialog == DialogType.ViewCameraDialog || data.activeDialog == DialogType.DebugOptions);
                    m_CachedActiveDialog = data.activeDialog;
                }
                //

                if (m_CurrentToolState != data.toolState)
                {
                    if (data.toolState.infoType == InfoType.Info)
                    {
                        b.SetIcon(m_InfoImage);
                    }
                    else if (data.toolState.infoType == InfoType.Debug)
                    {
                        b.SetIcon(m_DebugImage);
                    }
                    m_CurrentToolState = data.toolState;
                }
                b.button.interactable = data.toolbarsEnabled;
            }
        }

        private void ToggleUI(bool hide)
        {
            Debug.Log($"hide UI {hide}");
            hideUI = hide;
        }

        private void TakeScreenshot()
        {
            ScreenshotManager ss = GetComponentInChildren<ScreenshotManager>();
            ss.CreateScreenshot(uiRootCanvas, hideUI);
            //TODO: set up toggle bool for whether UI should be hidden in screenshot
        }

        
    }
}
