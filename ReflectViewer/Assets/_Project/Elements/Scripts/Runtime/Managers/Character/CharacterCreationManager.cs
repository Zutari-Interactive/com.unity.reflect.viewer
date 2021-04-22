using System;
using System.Collections.Generic;
using Elements.Character;
using Unity.Reflect.Viewer.UI;
using UnityEngine;

namespace Elements.Managers
{
    public class CharacterCreationManager : MonoBehaviour
    {
        #region VARIABLES

        [Header("Options")]
        public bool LoadOnStart = false;

        [Header("Character Manager")]
        public CharacterManager CharacterManager;

        [Header("Active Character")]
        public CharacterEntity Character;

        [Header("Character Data")]
        public CharacterData CharacterData;

        [Header("Skin Materials")]
        public List<Material> SkinMaterials = new List<Material>();

        [Header("Cloth Materials")]
        public List<Material> ClothesMaterials = new List<Material>();

        [Header("Accessory Materials")]
        public List<Material> AccessoryMaterials = new List<Material>();

        public static Action<int> OnChangeSkinMaterial;
        public static Action<int> OnChangeClothMaterial;
        public static Action<int> OnChangeAccessoryMaterial;

        #endregion

        #region UNITY METHODS

        private void Awake()
        {
            OnChangeSkinMaterial += ChangeSkinMaterial;
            OnChangeClothMaterial += ChangeClothMaterial;
            OnChangeAccessoryMaterial += ChangeAccessoryMaterial;
        }

        private void Start()
        {
            if (!LoadOnStart) return;
            RecreateExistingCharacter();
        }

        private void OnDestroy()
        {
            OnChangeSkinMaterial -= ChangeSkinMaterial;
            OnChangeClothMaterial -= ChangeClothMaterial;
            OnChangeAccessoryMaterial -= ChangeAccessoryMaterial;
        }

        #endregion

        #region CHARACTER METHODS

        public void ResetCharacter()
        {
            CharacterData.ResetCharacterData();

            ChangeSkinMaterial(CharacterData.SkinMaterialIndex);
            ChangeClothMaterial(CharacterData.ClothesMaterialIndex);
            ChangeAccessoryMaterial(CharacterData.AccessoryMaterialIndex);
        }

        public void RecreateExistingCharacter()
        {
            if (!CharacterData.FileExist()) return;

            LoadCharacterData();
            Character = CharacterManager.ActivateCharacter();
            CharacterManager.MoveToSpawnPoint();

            ChangeSkinMaterial(CharacterData.SkinMaterialIndex);
            ChangeClothMaterial(CharacterData.ClothesMaterialIndex);
            ChangeAccessoryMaterial(CharacterData.AccessoryMaterialIndex);
        }

        #endregion

        #region MATERIAL METHODS

        public void ChangeSkinMaterial(int index)
        {
            if (index < 0) return;
            Character = CharacterManager.Character;
            if (!Character) return;

            CharacterData.SkinMaterialIndex = index;
            Character.MaterialHandler.ChangeSkinColour(SkinMaterials[index]);
        }

        public void ChangeClothMaterial(int index)
        {
            if (index < 0) return;
            Character = CharacterManager.Character;
            if (!Character) return;

            CharacterData.ClothesMaterialIndex = index;
            Character.MaterialHandler.ChangeClothColour(ClothesMaterials[index]);
        }

        public void ChangeAccessoryMaterial(int index)
        {
            if (index < 0) return;
            Character = CharacterManager.Character;
            if (!Character) return;

            CharacterData.AccessoryMaterialIndex = index;
            Character.MaterialHandler.ChangeAccessoryColour(AccessoryMaterials[index]);
        }

        #endregion

        #region IO METHODS

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
