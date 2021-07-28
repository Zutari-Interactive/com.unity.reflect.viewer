using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

namespace Elements.Pointer
{
    public class SpawnPointer : NetworkedBehaviour
    {
        #region VARIABLES

        [Header("Network Pointer Prefab")]
        public NetworkedObject PointerPrefab;

        private Pointer _pointer;

        #endregion

        #region UNITY METHODS

        private void OnDestroy()
        {
            DestroyPointerServerRpc();
        }

        #endregion

        #region METHODS

        [ServerRPC]
        public void SpawnPointerServerRpc()
        {
            if (_pointer) return;
            NetworkedObject pointer = Instantiate(PointerPrefab);
            pointer.SpawnWithOwnership(OwnerClientId);

            _pointer = pointer.GetComponent<Pointer>();
        }

        [ServerRPC]
        public void DestroyPointerServerRpc()
        {
            if (!_pointer) return;
            Destroy(_pointer.gameObject);
        }

        #endregion
    }
}
