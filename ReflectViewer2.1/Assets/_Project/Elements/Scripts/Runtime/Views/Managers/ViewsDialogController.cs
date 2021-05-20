using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.TouchFramework;
using UnityEngine.Reflect;
using TMPro;

namespace Unity.Reflect.Viewer.UI
{

    [RequireComponent(typeof(DialogWindow))]
    public class ViewsDialogController : MonoBehaviour
    {
        ToolButton viewButton;

        DialogWindow m_DialogWindow;
        StatsInfoData m_CurrentStatsInfoData;
        DialogType m_CachedActiveDialog;
        ToolState m_CurrentToolState;

        void Awake()
        {
            UIStateManager.stateChanged += OnStateDataChanged;
            UIStateManager.debugStateChanged += OnDebugStateDataChanged;
            m_DialogWindow = GetComponent<DialogWindow>();
        }
    }
}

