using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkedVar;
using UnityEngine;

public class Pointer : NetworkedBehaviour
{
    #region VARIABLES

    public NetworkedVarVector3 Position = new NetworkedVarVector3();

    #endregion

    #region UNITY METHODS

    private void OnEnable()
    {
        // Start Listening for TeamIndex being updated
        Position.OnValueChanged += OnPositionChanged;
    }

    private void OnDisable()
    {
        // Stop Listening for TeamIndex being updated
        Position.OnValueChanged -= OnPositionChanged;
    }

    #endregion

    #region METHODS

    [ServerRPC]
    public void SetPointerPositionServerRpc(Vector3 position)
    {
        print($"Is Set Pointer Position - {position}");
        Position.Value = position;
        transform.position = position;
    }

    private void OnPositionChanged(Vector3 previousValue, Vector3 newValue)
    {
        if (!IsClient) return;
        transform.position = Position.Value;
    }

    #endregion
}
