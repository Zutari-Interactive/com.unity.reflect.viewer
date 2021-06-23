using System;
using Elements.LaserPointer;
using MLAPI;
using SharpFlux;
using SharpFlux.Dispatching;
using Unity.Reflect.Viewer.UI;
using UnityEngine;
using UnityEngine.Reflect.Viewer;
using UnityEngine.Reflect.Viewer.Pipeline;

namespace Elements.UI.Controllers
{
    public class LaserPointerUIController : MonoBehaviour
    {
        #region VARIABLES

#pragma warning disable 649
        [SerializeField]
        ToolButton m_LaserPointerButton;

        ISpatialPicker<Tuple<GameObject, RaycastHit>> m_ObjectPicker;


#pragma warning restore 649

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            m_LaserPointerButton.buttonClicked += UseLaserPointer;
            m_ObjectPicker = new SpatialSelector();
        }

        private void Start()
        {
            NetworkingManager.Singleton.OnClientConnectedCallback += ClientConnected;
            NetworkingManager.Singleton.OnClientDisconnectCallback += ClientDisconnected;
        }

        #endregion

        #region METHODS

        private void ClientConnected(ulong clientId)
        {
            m_LaserPointerButton.button.interactable = true;
        }

        private void ClientDisconnected(ulong clientId)
        {
            m_LaserPointerButton.button.interactable = false;
        }

        private void UseLaserPointer()
        {
            print("Find Client");
            Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.SetObjectPicker, m_ObjectPicker));
            if (NetworkingManager.Singleton.ConnectedClients.TryGetValue(NetworkingManager.Singleton.LocalClientId, out var networkedClient))
            {
                Pointer3D pointer3D = Camera.main.GetComponent<Pointer3D>();
                if (pointer3D)
                {
                    pointer3D.UsePointer3D(m_ObjectPicker);
                }
                else
                {
                    print("No Laser Found");
                }
            }
            else
            {
                print("No Clients Found");
            }
        }

        #endregion
    }
}
