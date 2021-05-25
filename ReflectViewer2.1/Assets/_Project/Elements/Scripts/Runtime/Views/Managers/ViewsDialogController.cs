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

namespace Unity.Reflect.Viewer.UI
{

    [RequireComponent(typeof(DialogWindow))]
    public class ViewsDialogController : MonoBehaviour
    {
        [SerializeField]
        ToolButton viewButton;

        public GameObject viewButtonPrefab;
        public Transform viewsParent;
        private List<ToolButton> views = new List<ToolButton>();
        private StaticViewCollection viewCollection;

        [SerializeField]
        Sprite m_InfoImage;
        [SerializeField]
        Sprite m_DebugImage;

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

        private void Start()
        {
            viewButton.buttonClicked += OnViewButtonClicked;
            viewButton.buttonLongPressed += OnViewButtonLongPressed;
        }

        private void OnViewButtonLongPressed()
        {
            Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, DialogType.ViewDialog));
        }

        private void OnViewButtonClicked()
        {
            viewCollection = FindObjectOfType<StaticViewCollection>();
            if(viewCollection == null)
            {
                Debug.LogError("No view collection found, exiting");
                return;
            }

            if (m_CurrentToolState.infoType == InfoType.Info)
            {
                var dialogType = m_DialogWindow.open ? DialogType.None : DialogType.ViewDialog;
                Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.OpenDialog, dialogType));

                if (!m_DialogWindow.open)
                {
                    viewCollection.ReturnToMainCamera();
                }
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
                viewButton.selected = (data.activeDialog == DialogType.ViewDialog || data.activeDialog == DialogType.DebugOptions);
                m_CachedActiveDialog = data.activeDialog;
            }
            //

            if (m_CurrentToolState != data.toolState)
            {
                if (data.toolState.infoType == InfoType.Info)
                {
                    viewButton.SetIcon(m_InfoImage);
                }
                else if (data.toolState.infoType == InfoType.Debug)
                {
                    viewButton.SetIcon(m_DebugImage);
                }
                m_CurrentToolState = data.toolState;
            }
            viewButton.button.interactable = data.toolbarsEnabled;
        }

        void OnDebugStateDataChanged(UIDebugStateData data)
        {
            Debug.LogWarning("debug on this dialog not currently available");
        }

        public void CreateToolButton()
        {
            GameObject newButton = Instantiate(viewButtonPrefab);
            newButton.transform.SetParent(viewsParent);
            ToolButton tool = newButton.GetComponentInChildren<ToolButton>();

            Button baseButton = tool.gameObject.GetComponent<Button>();
            baseButton.interactable = true;
            baseButton.onClick.AddListener(delegate { ButtonPressed(baseButton.gameObject); });

            View v = tool.gameObject.AddComponent<View>();
            v.CreateView(tool);
            views.Add(tool);

            TextMeshProUGUI name = newButton.GetComponentInChildren<TextMeshProUGUI>();
            name.text = $"View {views.Count}";
            v.index = views.Count - 1;

            Debug.Log("View Button Created");
        }

        private void ButtonPressed(GameObject obj)
        {
            int v = obj.GetComponent<View>().index;
            viewCollection.DeActivateCameraView();
            viewCollection.ActivateCameraView(v);
        }

        private void ViewButtonClicked()
        {
            Debug.Log("view selected");
        }
    }
}

