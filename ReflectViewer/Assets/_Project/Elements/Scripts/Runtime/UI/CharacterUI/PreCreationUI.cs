using Elements.Character;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elements.UI
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
            if (!CharacterData.FileExist()) return;
            CharacterData.OverwriteFromFile();
            SceneManager.LoadSceneAsync(ReflectScene, LoadSceneMode.Single);
        }

        #endregion
    }
}
