using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zutari.Managers;

namespace Zutari.UI
{
    public class CharacterSerializationUI : MonoBehaviour
    {
        #region VARIABLES

        [Header("Character Manager")]
        public CharacterCreationManager CharacterCreationManager;

        [Header("Buttons")]
        public Button CompleteButton;
        public Button ReloadButton;

        #endregion

        #region UNITY METHODS

        public void Awake()
        {
            CompleteButton.onClick.AddListener(() =>
            {
                if (!CharacterCreationManager.Character) return;
                CharacterCreationManager.SaveCharacter();
                SceneManager.UnloadSceneAsync(CharacterCreationManager.CharacterManager.CharacterCreationSceneName);
            });

            ReloadButton.onClick.AddListener(() => { CharacterCreationManager.RecreateExistingCharacter(); });
        }

        #endregion

        #region METHODS

        #endregion
    }
}
