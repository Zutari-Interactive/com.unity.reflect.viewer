using System;
using UnityEngine;
using Elements.General;

namespace Elements.LifeCycle
{
    [Serializable]
    public class AssetLifeCycleState
    {
        #region VARIABLES

        [Header("Asset State")]
        public AssetState AssetState = AssetState.RoutineMaintenance;

        [Header("Degradation Value")]
        public float DegradationValue = 4500f;

        [Header("Color Representation")]
        public Material Material;

        #endregion

        #region CONSTRUCTORS

        public AssetLifeCycleState()
        {
        }

        public AssetLifeCycleState(AssetState assetState, float degradationValue, Material material)
        {
            AssetState = assetState;
            DegradationValue = degradationValue;
            Material = material;
        }

        #endregion
    }
}
