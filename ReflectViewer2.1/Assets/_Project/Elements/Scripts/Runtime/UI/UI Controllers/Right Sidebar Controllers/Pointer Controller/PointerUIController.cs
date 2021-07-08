using System;
using Elements.LaserPointer;
using MLAPI;
using SharpFlux;
using SharpFlux.Dispatching;
using Unity.Reflect.Viewer.UI;
using UnityEngine;
using UnityEngine.Reflect.MeasureTool;
using UnityEngine.Reflect.Viewer;
using UnityEngine.Reflect.Viewer.Pipeline;

namespace Elements.UI.Controllers
{
    public class PointerUIController : NetworkedBehaviour
    {
        #region VARIABLES

#pragma warning disable 649
        [SerializeField]
        ToolButton m_PointerButton;

        ISpatialPicker<Tuple<GameObject, RaycastHit>> m_ObjectPicker;

        private Camera _camera;

#pragma warning restore 649

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            m_PointerButton.buttonClicked += UsePointer;
            m_ObjectPicker = new SpatialSelector();
            _camera = Camera.main;
        }

        private void Start()
        {
            NetworkingManager.Singleton.OnClientConnectedCallback += ClientConnected;
            NetworkingManager.Singleton.OnClientDisconnectCallback += ClientDisconnected;
        }

        #endregion

        #region NETWORK METHODS

        private void ClientConnected(ulong clientId)
        {
            m_PointerButton.button.interactable = true;
        }

        private void ClientDisconnected(ulong clientId)
        {
            m_PointerButton.button.interactable = false;
        }

        private void UsePointer()
        {
            Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.SetObjectPicker, m_ObjectPicker));

            if (NetworkingManager.Singleton.ConnectedClients.TryGetValue(NetworkingManager.Singleton.LocalClientId, out var networkedClient))
            {
                // ToDo : Send Boolean Value to UIPointerTool
            }
            else
            {
                print("No Clients Found");
            }
        }

        #endregion
    }
}
