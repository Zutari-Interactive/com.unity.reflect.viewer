using MLAPI;
using UnityEngine;

namespace Elements.LaserPointer
{
    public class PointerManager : MonoBehaviour
    {
        #region VARIABLES

        private Pointer3D pointer3D;

        #endregion

        #region UNITY METHODS

        private void Update()
        {
            CheckMouseInput();
        }

        #endregion

        #region METHODS

        private void CheckMouseInput()
        {
            if (Input.GetMouseButton(0))
            {
                if (NetworkingManager.Singleton.ConnectedClients.TryGetValue(NetworkingManager.Singleton.LocalClientId, out var networkedClient))
                {
                    if (!pointer3D) pointer3D = Camera.main.GetComponentInChildren<Pointer3D>();
                    pointer3D.GetMousePosition();
                }
            }
            else if (!Input.GetMouseButton(0))
            {
                if (pointer3D) pointer3D.SetPointerPositionToCacheValue();
            }
        }

        #endregion
    }
}
