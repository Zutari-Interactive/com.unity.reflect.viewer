using System.Collections;
using System.Collections.Generic;
using Unity.Reflect.Viewer.UI;
using UnityEngine;
using SharpFlux;
using SharpFlux.Dispatching;
using Unity.TouchFramework;
using System;

[RequireComponent(typeof(DialogWindow))]
public class DataPackOptionsDialogController : MonoBehaviour
{
    [SerializeField]
    ToolButton dataInfoButton;
    [SerializeField]
    ToolButton iotButton;
    [SerializeField]
    ToolButton manualButton;
    [SerializeField]
    ToolButton photoButton;
    [SerializeField]
    ToolButton reportButton;
    [SerializeField]
    ToolButton exitButton;

    [SerializeField]
    Sprite m_InfoImage;
    [SerializeField]
    Sprite m_DebugImage;

    public IOTSensorGroupDisplay IOTGroupDisplay;
    private IOTSensorGroup currentSensorGroup;
    private DataPack currentDataPack;

    DialogWindow m_DialogWindow;
    StatsInfoData m_CurrentStatsInfoData;
    DialogType m_CachedActiveDialog;
    ToolState m_CurrentToolState;


    private void Awake()
    {
        UIStateManager.stateChanged += OnStateDataChanged;
        UIStateManager.debugStateChanged += OnDebugStateDataChanged;
        m_DialogWindow = GetComponent<DialogWindow>();
    }

    

    // Start is called before the first frame update
    void Start()
    {
        dataInfoButton.buttonClicked += OnDataInfoButtonClicked;
        iotButton.buttonClicked += OnIOTButtonClicked;
        manualButton.buttonClicked += OnManualInfoButtonClicked;
        photoButton.buttonClicked += OnPhotoButtonClicked;
        reportButton.buttonClicked += OnReportButtonClicked;
        exitButton.buttonClicked += OnDataInfoButtonClicked;
    }

    

    private void OnDebugStateDataChanged(UIDebugStateData data)
    {
        throw new NotImplementedException();
    }

    private void OnStateDataChanged(UIStateData data)
    {
        if (m_CachedActiveDialog != data.activeDialog)
        {
            dataInfoButton.selected = (data.activeDialog == DialogType.IOTData || data.activeDialog == DialogType.DebugOptions);
            m_CachedActiveDialog = data.activeDialog;
        }
        //

        if (m_CurrentToolState != data.toolState)
        {
            if (data.toolState.infoType == InfoType.Info)
            {
                dataInfoButton.SetIcon(m_InfoImage);
            }
            else if (data.toolState.infoType == InfoType.Debug)
            {
                dataInfoButton.SetIcon(m_DebugImage);
            }
            m_CurrentToolState = data.toolState;
        }
    }

    private void OnDataInfoButtonClicked()
    {
        if (m_CurrentToolState.infoType == InfoType.Info)
        {
            var dialogType = m_DialogWindow.open ? DialogType.None : DialogType.IOTData;
            Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));

        }
        if (m_CurrentToolState.infoType == InfoType.Debug)
        {
            //var dialogType = m_DebugDialogWindow.open ? DialogType.None : DialogType.DebugOptions;
            //Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));
            Debug.LogWarning("no debug dialog available currently");
        }
    }

    private void OnIOTButtonClicked()
    {
        if (m_CurrentToolState.infoType == InfoType.Info)
        {
            var dialogType = m_DialogWindow.open ? DialogType.None : DialogType.IOTDataGroup;
            Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));

            IOTGroupDisplay.SetupGroupUI(currentSensorGroup);
        }
        if (m_CurrentToolState.infoType == InfoType.Debug)
        {
            //var dialogType = m_DebugDialogWindow.open ? DialogType.None : DialogType.DebugOptions;
            //Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));
            Debug.LogWarning("no debug dialog available currently");
        }
    }

    private void OnManualInfoButtonClicked()
    {
        DataPath path = new DataPath();
        path.AssignPath(currentDataPack.paths[0]);
    }

    private void OnPhotoButtonClicked()
    {
        DataPath path = new DataPath();
        path.AssignPath(currentDataPack.paths[1]);
    }

    private void OnReportButtonClicked()
    {
        DataPath path = new DataPath();
        path.AssignPath(currentDataPack.paths[2]);
    }

    public void SetSensorGroup(IOTSensorGroup group)
    {
        currentSensorGroup = group;
    }

    public void SetDataPack(DataPack pack)
    {
        currentDataPack = pack;
    }

    private void OnDisable()
    {
        dataInfoButton.buttonClicked -= OnDataInfoButtonClicked;
        iotButton.buttonClicked -= OnIOTButtonClicked;
        manualButton.buttonClicked -= OnManualInfoButtonClicked;
        photoButton.buttonClicked -= OnPhotoButtonClicked;
        reportButton.buttonClicked-= OnReportButtonClicked;
        exitButton.buttonClicked -= OnDataInfoButtonClicked;
    }
}
