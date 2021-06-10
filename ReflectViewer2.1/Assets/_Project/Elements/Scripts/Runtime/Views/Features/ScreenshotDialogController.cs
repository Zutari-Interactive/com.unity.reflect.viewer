using SharpFlux;
using SharpFlux.Dispatching;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Reflect.Viewer.UI;
using Unity.TouchFramework;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(DialogWindow))]
public class ScreenshotDialogController : MonoBehaviour
{
    [SerializeField]
    ToolButton screenshotButton;
    [SerializeField]
    ToolButton takeScreenshotButton;
    [SerializeField]
    ToolButton closeDialogButton;
    [SerializeField]
    SlideToggle hideUIToggle;

    [SerializeField]
    Sprite m_InfoImage;
    [SerializeField]
    Sprite m_DebugImage;

    DialogWindow m_DialogWindow;
    StatsInfoData m_CurrentStatsInfoData;
    DialogType m_CachedActiveDialog;
    ToolState m_CurrentToolState;

    public Canvas uiRootCanvas;
    public GameObject viewerWindow;
    public bool availableInVR;

    private Camera handCamera;

    private bool hideUI;

    void Awake()
    {
        UIStateManager.stateChanged += OnStateDataChanged;
        UIStateManager.debugStateChanged += OnDebugStateDataChanged;
        m_DialogWindow = GetComponent<DialogWindow>();

        
        
    }

    //TODO - if not available in VR, hide the screenshot button in left UI cluster 
    private void SetupVRScreenshotCam()
    {
        
        XRRig rig = FindObjectOfType<XRRig>();
        if (rig != null)
        {
            handCamera = GetComponentInChildren<Camera>();

            if (handCamera == null)
            {
                Debug.LogError("no camera found for VR screenshot tool");
                return;
            }
            viewerWindow.SetActive(true);
            ActivateCamera(true);
        }
        else
        {
            viewerWindow.SetActive(false);
            ActivateCamera(false);
        }
        
    }

    private void Start()
    {
        screenshotButton.buttonClicked += OnScreenShotButtonClicked;
        screenshotButton.buttonLongPressed += OnScreenshotButtonLongPressed;
        takeScreenshotButton.buttonClicked += TakeScreenshot;
        closeDialogButton.buttonClicked += OnScreenShotButtonClicked;
        hideUIToggle.onValueChanged.AddListener(ToggleUI);

        CheckUIToggle();
    }

    private void CheckUIToggle()
    {
        if (hideUIToggle.on)
        {
            ToggleUI(true);
        }
    }


    private void OnScreenshotButtonLongPressed()
    {
        Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, DialogType.Screenshot));
    }

    private void OnScreenShotButtonClicked()
    {
        if (m_CurrentToolState.infoType == InfoType.Info)
        {
            var dialogType = m_DialogWindow.open ? DialogType.None : DialogType.Screenshot;
            Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));

            if(availableInVR)
                SetupVRScreenshotCam();
            
        }
        if (m_CurrentToolState.infoType == InfoType.Debug)
        {
            //var dialogType = m_DebugDialogWindow.open ? DialogType.None : DialogType.DebugOptions;
            //Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));
            Debug.LogWarning("no debug dialog available currently");
        }
    }

    void OnStateDataChanged(UIStateData data)
    {
        if (m_CachedActiveDialog != data.activeDialog)
        {
            screenshotButton.selected = (data.activeDialog == DialogType.Screenshot || data.activeDialog == DialogType.DebugOptions);
            m_CachedActiveDialog = data.activeDialog;
        }
        //

        if (m_CurrentToolState != data.toolState)
        {
            if (data.toolState.infoType == InfoType.Info)
            {
                screenshotButton.SetIcon(m_InfoImage);
            }
            else if (data.toolState.infoType == InfoType.Debug)
            {
                screenshotButton.SetIcon(m_DebugImage);
            }
            m_CurrentToolState = data.toolState;
        }
        screenshotButton.button.interactable = data.toolbarsEnabled;
    }

    void OnDebugStateDataChanged(UIDebugStateData data)
    {
        Debug.LogWarning("debug on this dialog not currently available");
    }

    private void ToggleUI(bool hide)
    {
        Debug.Log($"hide UI {hide}");
        hideUI = hide;
    }

    private void TakeScreenshot()
    {
        ScreenshotManager ss = GetComponent<ScreenshotManager>();
        ss.CreateScreenshot(uiRootCanvas, hideUI);
    }

    public void ActivateCamera(bool value)
    {
        if(handCamera != null)
        {
            handCamera.enabled = value;
        }
    }

    private void OnDisable()
    {
        screenshotButton.buttonClicked -= OnScreenShotButtonClicked;
        screenshotButton.buttonLongPressed -= OnScreenshotButtonLongPressed;
        takeScreenshotButton.buttonClicked -= TakeScreenshot;
        closeDialogButton.buttonClicked -= OnScreenShotButtonClicked;
        hideUIToggle.onValueChanged.RemoveListener(ToggleUI);
    }
}
