using MLAPI;
using UnityEngine;

namespace Elements.LaserPointer.Test
{
    public class HelloWorldManager : MonoBehaviour
    {
        private static string Id;
        private static Pointer3D player;

        void OnGUI()
        {
            GUILayout.BeginArea(new Rect(10, 10, 300, 300));
            if (!NetworkingManager.Singleton.IsClient && !NetworkingManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();

                //SubmitNewPosition();
            }

            GUILayout.EndArea();
        }

        private void Update()
        {
            CheckMouseInput();
        }

        static void StartButtons()
        {
            if (GUILayout.Button("Host")) NetworkingManager.Singleton.StartHost();
            if (GUILayout.Button("Client")) NetworkingManager.Singleton.StartClient();
            if (GUILayout.Button("Server")) NetworkingManager.Singleton.StartServer();
        }

        static void StatusLabels()
        {
            var mode = NetworkingManager.Singleton.IsHost ? "Host" : NetworkingManager.Singleton.IsServer ? "Server" : "Client";
            GUILayout.Label("Transport: " + NetworkingManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: "      + mode);
            GUILayout.Label("Client ID: " + Id);
        }

        static void CheckMouseInput()
        {
            if (Input.GetMouseButton(0))
            {
                if (NetworkingManager.Singleton.ConnectedClients.TryGetValue(NetworkingManager.Singleton.LocalClientId, out var networkedClient))
                {
                    player = networkedClient.PlayerObject.GetComponent<Pointer3D>();
                    if (player)
                    {
                        Id = NetworkingManager.Singleton.LocalClientId.ToString();
                        player.GetMousePosition();
                    }
                }
            }
            else if (!Input.GetMouseButton(0))
            {
                if (player) player.PointerPosition.Value = Vector3.zero;
            }
        }
    }
}
