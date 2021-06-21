using System;
using System.Collections.Generic;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.NetworkedVar;
using UnityEngine;
using UnityEngine.Reflect.Viewer.Pipeline;

namespace Elements.LaserPointer
{
    public class Pointer3D : NetworkedBehaviour
    {
        #region VARIABLES

        [Header("Pointer Renderer")]
        public LineRenderer Pointer;

        [Header("Networked Variables")]
        public NetworkedVarVector3 PointerPosition = new NetworkedVarVector3(new NetworkedVarSettings
        {
            WritePermission = NetworkedVarPermission.OwnerOnly,
            ReadPermission = NetworkedVarPermission.Everyone
        });

        public NetworkedVarBool UsePointer = new NetworkedVarBool(new NetworkedVarSettings()
        {
            WritePermission = NetworkedVarPermission.OwnerOnly,
            ReadPermission = NetworkedVarPermission.Everyone
        });

        [Header("Hand Transform")]
        public Transform m_HandTransform;

        readonly List<Tuple<GameObject, RaycastHit>> m_Results = new List<Tuple<GameObject, RaycastHit>>();

        private Camera _camera;
        private Ray _ray;
        private Vector3 _pointer = Vector3.zero;
        private RaycastHit _rayHit;
        private bool _usePointer = false;

        private ISpatialPicker<Tuple<GameObject, RaycastHit>> m_ObjectPicker;

        #endregion

        #region UNITY METHODS

        public override void NetworkStart()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            Pointer.SetPosition(1, PointerPosition.Value);
        }

        #endregion

        #region METHODS

        public void UsePointer3D(ISpatialPicker<Tuple<GameObject, RaycastHit>> objectPicker)
        {
            if (IsClient)
            {
                UsePointer.Value = CanUsePointer();
                m_ObjectPicker = objectPicker;
            }
            else
            {
                UsePointer3DServerRpc();
                m_ObjectPicker = objectPicker;
            }
        }

        [ServerRPC]
        public void UsePointer3DServerRpc()
        {
            UsePointer.Value = CanUsePointer();
        }

        public bool CanUsePointer()
        {
            _usePointer = !_usePointer;
            return _usePointer;
        }

        public void GetMousePosition()
        {
            if (IsClient)
            {
                if (!UsePointer.Value) return;
                PointerPosition.Value = MousePointer();
                Pointer.SetPosition(1, PointerPosition.Value);
            }
            else
            {
                if (!UsePointer.Value) return;
                GetMousePositionServerRpc();
            }
        }

        [ServerRPC]
        public void GetMousePositionServerRpc()
        {
            PointerPosition.Value = MousePointer();
        }


        public Vector3 MousePointer()
        {
            _ray.origin = m_HandTransform.position;
            _ray.direction = m_HandTransform.forward;

            // _ray = _camera.ScreenPointToRay(Input.mousePosition);
            // if (!Physics.Raycast(_ray, out _rayHit)) return new Vector3(0f, 0f, 0f);
            // _pointer = _rayHit.point - transform.position;
            // _pointer.y = 0f;

            m_Results.Clear();
            m_ObjectPicker.VRPick(_ray, m_Results);

            if (m_Results.Count == 0)
                return Vector3.zero;

            _pointer = m_Results[0].Item2.point;

            return _pointer;
        }

        #endregion
    }
}
