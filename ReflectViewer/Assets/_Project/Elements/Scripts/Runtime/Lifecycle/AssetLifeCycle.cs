using System;
using System.Collections;
using System.Collections.Generic;
using Unity.TouchFramework;
using UnityEngine;
using Zutari.Database;

namespace Zutari.LifeCycle
{
    public enum AssetState
    {
        New = 0,
        RoutineMaintenance = 1,
        SpecificOrMinorMaintenance = 2,
        SignificantOrMajorRepair = 3,
        Replace = 4,
    }

    [Serializable]
    public class LifeCycleState
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

        public LifeCycleState()
        {
        }

        public LifeCycleState(AssetState assetState, float degradationValue, Material material)
        {
            AssetState = assetState;
            DegradationValue = degradationValue;
            Material = material;
        }

        #endregion
    }

    public class AssetLifeCycle : MonoBehaviour
    {
        #region VARIABLES

        [Header("Current Life Cycle")]
        public LifeCycleState CurrentLifeCycle = new LifeCycleState();

        [Header("Life Cycle Period")]
        public LifeCycleDictionary LifeCycleDictionary = new LifeCycleDictionary();

        private Renderer _renderer;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
        }

        private void OnEnable()
        {
            AssetLifeCycleSliderControl.OnYearChanged += OnYearChanged;
        }

        private void OnDisable()
        {
            AssetLifeCycleSliderControl.OnYearChanged -= OnYearChanged;
        }

        #endregion

        #region METHODS

        public void OnYearChanged(int value)
        {
            if (!_renderer) _renderer = GetComponent<Renderer>();
            CurrentLifeCycle = LifeCycleDictionary[value];
            _renderer.material = CurrentLifeCycle.Material;
        }

        #endregion
    }
}
