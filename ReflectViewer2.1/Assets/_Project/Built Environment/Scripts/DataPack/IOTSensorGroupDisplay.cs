 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Reflect.Viewer.UI;
using Unity.TouchFramework;
using SharpFlux;
using SharpFlux.Dispatching;
using System;

public class IOTSensorGroupDisplay : MonoBehaviour
{
    [SerializeField]
    ToolButton iotButton;

    [SerializeField]
    Sprite m_InfoImage;
    [SerializeField]
    Sprite m_DebugImage;

    DialogWindow m_DialogWindow;
    StatsInfoData m_CurrentStatsInfoData;
    DialogType m_CachedActiveDialog;
    ToolState m_CurrentToolState;

    public TextMeshProUGUI groupNameText;
    public List<TextMeshProUGUI> sensorNames = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> sensorValues = new List<TextMeshProUGUI>();

    private IOTSensorGroup currentSensorGroup;

    private void Awake()
    {
        UIStateManager.stateChanged += OnStateDataChanged;
        UIStateManager.debugStateChanged += OnDebugStateDataChanged;
        m_DialogWindow = GetComponent<DialogWindow>();
    }

    private void OnDebugStateDataChanged(UIDebugStateData obj)
    {
        Debug.LogWarning("debug on this dialog not currently available");
    }

    private void OnStateDataChanged(UIStateData data)
    {
        if (m_CachedActiveDialog != data.activeDialog)
        {
            iotButton.selected = (data.activeDialog == DialogType.IOTDataGroup || data.activeDialog == DialogType.DebugOptions);
            m_CachedActiveDialog = data.activeDialog;
        }
        //

        if (m_CurrentToolState != data.toolState)
        {
            if (data.toolState.infoType == InfoType.Info)
            {
                iotButton.SetIcon(m_InfoImage);
            }
            else if (data.toolState.infoType == InfoType.Debug)
            {
                iotButton.SetIcon(m_DebugImage);
            }
            m_CurrentToolState = data.toolState;
        }
    }

    private void Start()
    {

        iotButton.buttonClicked += OnIOTButtonClicked;
    }

    private void OnIOTButtonClicked()
    {
        Debug.Log("Open Sensor Group Dialog");
        if (m_CurrentToolState.infoType == InfoType.Info)
        {
            var dialogType = m_DialogWindow.open ? DialogType.None : DialogType.IOTDataGroup;
            Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));

            SetupGroupUI();
        }
        if (m_CurrentToolState.infoType == InfoType.Debug)
        {
            //var dialogType = m_DebugDialogWindow.open ? DialogType.None : DialogType.DebugOptions;
            //Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));
            Debug.LogWarning("no debug dialog available currently");
        }
    }

    public void SetupGroupUI()
    {
        groupNameText.text = currentSensorGroup.groupName;

        for (int i = 0; i < currentSensorGroup.sensorGroup.Count; i++)
        {

            sensorNames[i].text = currentSensorGroup.sensorGroup[i].SensorName;
            
            sensorValues[i].text = currentSensorGroup.sensorGroup[i].SensorValue;
        }

        for (int i = sensorNames.Count -1; i == currentSensorGroup.sensorGroup.Count; i--)
        {
            sensorNames[i].text = "";
            sensorValues[i].text = "";
        }

        StartUpdatingSensorValues();
    }

    private void StartUpdatingSensorValues()
    {
        for (int i = 0; i < currentSensorGroup.sensorGroup.Count; i++)
        {
            currentSensorGroup.sensorGroup[i].update = true;
        }
    }

    public void SetSensorGroup(IOTSensorGroup group)
    {
        currentSensorGroup = group;
    }
}
