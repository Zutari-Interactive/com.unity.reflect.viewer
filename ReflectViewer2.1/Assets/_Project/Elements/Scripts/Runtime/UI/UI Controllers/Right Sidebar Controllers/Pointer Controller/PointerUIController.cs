using System;
using MLAPI;
using MLAPI.Messaging;
using SharpFlux;
using SharpFlux.Dispatching;
using Unity.Reflect.Viewer.UI;
using UnityEngine;
using UnityEngine.Reflect.Viewer;
using UnityEngine.Reflect.Viewer.Pipeline;

namespace Elements.UI.Controllers
{
    public class PointerUIController : NetworkedBehaviour
    {
        #region VARIABLES

#pragma warning disable 649
        [Header("Pointer Prefab")]
        public NetworkedObject PointerPrefab;

        [Header("Pointer Button")]
        public ToolButton m_PointerButton;

        private ISpatialPicker<Tuple<GameObject, RaycastHit>> m_ObjectPicker;
        private NetworkedObject _networkedPointer;

        private Camera _camera;
        private Pointer _pointer;

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
            // NetworkingManager.Singleton.OnClientConnectedCallback += ClientConnected;
            // NetworkingManager.Singleton.OnClientDisconnectCallback += ClientDisconnected;
        }

        private void Update()
        {
            // Make sure Pointer exists
            if (!_networkedPointer) return;
            print("Has Networked Pointer");

            // Get the Pointer Component
            if (!_pointer) _networkedPointer.TryGetComponent(out _pointer);
            print("Has Pointer Component");

            // Make sure we press the mouse button
            if (!Input.GetMouseButtonDown(0)) return;
            print("Has Mouse Down");

            // Find a collision
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) return;
            print("Has Ray Cast Hit");
            print(hit.point);

            // Send a message to the Server Update Pointer Position
            UpdatePointerPositionServerRpc(hit.point);
        }

        #endregion

        #region NETWORK METHODS

        private void ClientConnected(ulong clientId)
        {
            print("Client Connected");
            print($"Number of Clients - {NetworkingManager.Singleton.ConnectedClientsList.Count}");
            m_PointerButton.button.interactable = true;
            SpawnPointerServerRpc();
        }

        private void ClientDisconnected(ulong clientId)
        {
            print("Client Disconnected");
            print($"Number of Clients - {NetworkingManager.Singleton.ConnectedClientsList.Count}");
            m_PointerButton.button.interactable = false;
            UnspawnPointerServerRpc();
        }

        private void UsePointer()
        {
            print("Dispatch Object Picker Payload!");
            Dispatcher.Dispatch(Payload<ActionTypes>.From(ActionTypes.SetObjectPicker, m_ObjectPicker));
            SpawnPointerServerRpc();

            // ToDo : Get Position of Collision
        }

        [ServerRPC]
        private void SpawnPointerServerRpc()
        {
            if (!_networkedPointer) _networkedPointer = Instantiate(PointerPrefab);
            _networkedPointer.Spawn();
        }

        [ServerRPC]
        private void UnspawnPointerServerRpc()
        {
            if (!_networkedPointer) return;
            _networkedPointer.UnSpawn();
        }

        [ServerRPC]
        private void UpdatePointerPositionServerRpc(Vector3 position)
        {
            print($"Update Pointer Position - {position}");
            _pointer.SetPointerPositionServerRpc(position);
        }

        #endregion
    }
}
