using Elements.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Elements.UI
{
    public class SelectGenderUI : MonoBehaviour
    {
        #region VARIABLES

        [Header("Character Manager")]
        public CharacterManager CharacterManager;

        [Header("Selection Buttons")]
        public Button SelectMaleButton;
        public Button SelectFemaleButton;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            SelectMaleButton.onClick.AddListener(OnClickSelectMale);
            SelectFemaleButton.onClick.AddListener(OnClickSelectFemale);
        }

        #endregion

        #region METHODS

        public void OnClickSelectMale()
        {
            CharacterManager.SetCharacterIndex(1);
            CharacterManager.ActivateCharacter();
        }

        public void OnClickSelectFemale()
        {
            CharacterManager.SetCharacterIndex(0);
            CharacterManager.ActivateCharacter();
        }

        #endregion
    }
}
