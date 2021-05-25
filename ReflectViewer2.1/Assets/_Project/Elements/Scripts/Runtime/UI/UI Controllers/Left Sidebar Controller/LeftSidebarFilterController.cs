using Unity.Reflect.Viewer.UI;
using UnityEngine;

namespace Elements.UI.Controllers
{
    public class LeftSidebarFilterController : MonoBehaviour
    {
        #region VARIABLES

        [Header("Filter Buttons")]
        public ToolButton AirTerminalFilterButton;

        public static LeftSidebarFilterController Instance;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            if (!Instance) Instance = this;
        }

        #endregion

        #region METHODS

        public void ActivateAirTerminalFilterButton(bool value)
        {
            AirTerminalFilterButton.gameObject.SetActive(value);
        }

        #endregion
    }
}
