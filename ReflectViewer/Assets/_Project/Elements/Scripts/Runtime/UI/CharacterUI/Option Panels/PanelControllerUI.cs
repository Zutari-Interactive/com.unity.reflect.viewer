using UnityEngine;
using UnityEngine.UI;

namespace Zutari.UI
{
    public class PanelControllerUI : MonoBehaviour
    {
        #region VARIABLES

        [Header("Buttons")]
        public Button Panel1Button;
        public Button Panel2Button;
        public Button Panel3Button;

        [Header("Panels")]
        public GameObject CurrentActivePanel;
        public GameObject Panel1;
        public GameObject Panel2;
        public GameObject Panel3;

        #endregion

        #region UNITY METHODS

        public void Start()
        {
            if (!CurrentActivePanel) CurrentActivePanel = Panel1;

            Panel1Button.onClick.AddListener(() =>
            {
                CurrentActivePanel.SetActive(false);
                CurrentActivePanel = Panel1;
                CurrentActivePanel.SetActive(true);
            });

            Panel2Button.onClick.AddListener(() =>
            {
                CurrentActivePanel.SetActive(false);
                CurrentActivePanel = Panel2;
                CurrentActivePanel.SetActive(true);
            });

            Panel3Button.onClick.AddListener(() =>
            {
                CurrentActivePanel.SetActive(false);
                CurrentActivePanel = Panel3;
                CurrentActivePanel.SetActive(true);
            });

            CurrentActivePanel.SetActive(true);
        }

        #endregion
    }
}
