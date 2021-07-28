using Elements.Pointer;
using MLAPI;
using MLAPI.Connection;
using Unity.Reflect.Viewer.UI;
using UnityEngine;

namespace Elements.UI.Controllers
{
    public class PointerUIController : MonoBehaviour
    {
        #region VARIABLES

#pragma warning disable 649

        [Header("Pointer Button")]
        public ToolButton m_PointerButton;

        private bool _usePointer = false;
        private SpawnPointer _localSpawnPointer;

#pragma warning restore 649

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            m_PointerButton.buttonClicked += UsePointer;
        }

        private void Start()
        {
            // NetworkingManager.Singleton.OnClientConnectedCallback += OnClientConnected;
            // NetworkingManager.Singleton.OnClientDisconnectCallback += OnClientDisconnected;
        }

        #endregion

        #region METHODS

        private void UsePointer()
        {
            _usePointer = !_usePointer;
            FindLocalSpawnPointer();
            if (!_usePointer)
            {
                _localSpawnPointer?.DestroyPointerServerRpc();
            }
            else if (_usePointer)
            {
                _localSpawnPointer?.SpawnPointerServerRpc();
            }
        }

        private void FindLocalSpawnPointer()
        {
            // Check if we already have a local spawn pointer
            if (_localSpawnPointer) return;

            // Check if we are connected
            if (!NetworkingManager.Singleton.IsConnectedClient) return;

            // Get Local Client ID
            ulong localClientId = NetworkingManager.Singleton.LocalClientId;

            // Try Get Local Client Object using LocalClientID
            if (!NetworkingManager.Singleton.ConnectedClients.TryGetValue(localClientId, out NetworkedClient networkClient)) return;

            // Try Get TeamPlayer Component from Local Client Object
            if (!networkClient.PlayerObject.TryGetComponent<SpawnPointer>(out _localSpawnPointer)) return;

            // Send a Message to the Server to Spawn the Pointer Network Object
            _localSpawnPointer.SpawnPointerServerRpc();
        }

        #endregion

        #region NETWORK METHODS

        private void OnClientConnected(ulong clientId)
        {
            print("Client Connected");
            m_PointerButton.button.interactable = true;
        }

        private void OnClientDisconnected(ulong clientId)
        {
            print("Client Diconnected");
            m_PointerButton.button.interactable = false;
        }

        #endregion
    }
}
