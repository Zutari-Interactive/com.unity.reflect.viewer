using UnityEngine;
using UnityEngine.SceneManagement;
using Zutari.Character;

namespace Zutari.UI
{
    public class PreCreationUI : MonoBehaviour
    {
        #region VARIABLES

        [Header("Character Data")]
        public CharacterData CharacterData;

        [Header("Gender Selection Panel")]

        [Header("Reflect Scene")]
        public string ReflectScene;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            if (!CharacterData.Exists()) return;
            CharacterData.OverwriteFromFile();
            SceneManager.LoadSceneAsync(ReflectScene, LoadSceneMode.Single);
        }

        #endregion
    }
}
