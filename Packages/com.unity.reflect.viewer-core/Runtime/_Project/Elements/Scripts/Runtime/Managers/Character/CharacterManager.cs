using System;
using System.Collections.Generic;
using Elements.Character;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Elements.Managers
{
    [DefaultExecutionOrder(-1)]
    public class CharacterManager : MonoBehaviour
    {
        #region VARIABLES

        [Header("Options")]
        public bool LoadOnStart = false;

        [Header("Character Scene")]
        public string CharacterCreationSceneName = "";

        [Header("Characters")]
        public List<CharacterEntity> Characters = new List<CharacterEntity>();

        [Header("Active Character")]
        public CharacterEntity Character;

        [Header("Character Data")]
        public CharacterData CharacterData;

        [Header("Spawn Point")]
        public Transform SpawnPoint;


        public static Action<int> OnSetCharacterIndex;
        public static Func<CharacterEntity> OnActivateCharacter;
        public static Action OnDeactivateCharacter;
        public static Action OnDeactivateAllCharacters;
        public static Action<Vector3> OnPlaceSpawnPoint;
        public static Action OnMoveToSpawnPoint;

        #endregion

        #region PROPERTIES

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            OnSetCharacterIndex += SetCharacterIndex;
            OnActivateCharacter += ActivateCharacter;
            OnDeactivateCharacter += DeactivateCharacter;
            OnDeactivateAllCharacters += DeactivateAll;
            OnPlaceSpawnPoint += PlaceSpawnPoint;
            OnMoveToSpawnPoint += MoveToSpawnPoint;
        }

        private void Start()
        {
            if (!LoadOnStart) return;
            LoadExistingCharacter();
        }

        private void OnDestroy()
        {
            OnSetCharacterIndex -= SetCharacterIndex;
            OnActivateCharacter -= ActivateCharacter;
            OnDeactivateCharacter -= DeactivateCharacter;
            OnDeactivateAllCharacters -= DeactivateAll;
            OnPlaceSpawnPoint -= PlaceSpawnPoint;
            OnMoveToSpawnPoint -= MoveToSpawnPoint;
        }

        #endregion

        #region METHODS

        public void LoadExistingCharacter()
        {
            if (!CharacterData.FileExist())
            {
                SceneManager.LoadSceneAsync(CharacterCreationSceneName, LoadSceneMode.Additive);
                return;
            }

            LoadCharacterData();
        }

        public CharacterEntity ActivateCharacter()
        {
            if (CharacterData.Gender < 0) return null;
            if (!Character) Character = Characters[CharacterData.Gender];
            else
            {
                Character.Deactivate();
                Character = Characters[CharacterData.Gender];
            }

            Character.Activate();
            return Character;
        }

        public void DeactivateCharacter()
        {
            Character.Deactivate();
        }

        public void DeactivateAll()
        {
            for (int i = 0; i < Characters.Count; i++)
            {
                Characters[i].Deactivate();
            }

            Character.Deactivate();
        }

        public void PlaceSpawnPoint(Vector3 position)
        {
            SpawnPoint.position = position;
        }

        public void MoveToSpawnPoint()
        {
            Character.transform.position = SpawnPoint.position;
        }

        public void SetCharacterIndex(int value)
        {
            if (value < 0 || value >= Characters.Count)
            {
                Debug.LogWarning("Invalid Character Index.");
            }

            CharacterData.Gender = value;
        }

        public void SaveCharacter()
        {
            CharacterData.WriteToFile();
        }

        public void LoadCharacterData()
        {
            CharacterData.OverwriteFromFile();
        }

        #endregion
    }
}
