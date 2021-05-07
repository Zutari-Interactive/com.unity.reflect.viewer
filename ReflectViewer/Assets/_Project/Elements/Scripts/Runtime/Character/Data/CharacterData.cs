﻿using SerializableSO;
using UnityEngine;

namespace Elements.Character
{
    // [CreateAssetMenu]
    public class CharacterData : SerializableSOBase
    {
        #region VARIABLES

        [Header("Username")]
        public string Username = "Zutari.Guest-Player";

        [Header("Character Gender")]
        public int Gender = -1;

        [Header("Skin Colour")]
        public int SkinMaterialIndex = -1;

        [Header("Cloth Color")]
        public int ClothesMaterialIndex = -1;

        [Header("Accessory Color")]
        public int AccessoryMaterialIndex = -1;

        #endregion

        #region PROPERTIES

        #endregion

        #region METHODS

        public void ResetCharacterData()
        {
            SkinMaterialIndex = -1;
            ClothesMaterialIndex = -1;
            AccessoryMaterialIndex = -1;
        }

        #endregion
    }
}
