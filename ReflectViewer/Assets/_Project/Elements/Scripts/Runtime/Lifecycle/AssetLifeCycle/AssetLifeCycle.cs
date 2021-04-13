using Unity.TouchFramework;
using UnityEngine;

namespace Zutari.LifeCycle
{
    public class AssetLifeCycle : MonoBehaviour
    {
        #region VARIABLES

        [Header("Current Life Cycle")]
        public AssetLifeCycleState CurrentLifeCycle = new AssetLifeCycleState();

        [Header("Life Cycle Period")]
        public AssetLifeCycleDictionary LifeCycleDictionary = new AssetLifeCycleDictionary();

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
