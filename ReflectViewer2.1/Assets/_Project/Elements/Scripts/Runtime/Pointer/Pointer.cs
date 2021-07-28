using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkedVar;
using UnityEngine;

namespace Elements.Pointer
{
    public class Pointer : NetworkedBehaviour
    {
        #region VARIABLES

        private NetworkedVarVector3 _position = new NetworkedVarVector3();

        private Camera _camera;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            _position.OnValueChanged += OnPositionChange;
        }

        private void Update()
        {
            // Make sure this belongs to us
            if (!IsOwner) return;

            // Make sure we press the mouse button
            if (!Input.GetMouseButtonDown(0)) return;

            // Find a collision
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) return;

            // Send a message to the Server to Change the Ball Position.
            SetPositionServerRpc(hit.point);
        }

        private void OnDisable()
        {
            _position.OnValueChanged -= OnPositionChange;
        }

        #endregion

        #region METHODS

        [ServerRPC]
        public void SetPositionServerRpc(Vector3 position)
        {
            SetPositionClientRpc(position);
        }

        [ClientRPC]
        public void SetPositionClientRpc(Vector3 position)
        {
            _position.Value = position;
        }

        private void OnPositionChange(Vector3 previousvalue, Vector3 newvalue)
        {
            transform.position = _position.Value;
        }

        #endregion
    }
}
