using System;
using System.Collections.Generic;
using UnityEngine;
using Zutari.Managers;

namespace Zutari.UI
{
    public abstract class ColourSelectionBaseUI : MonoBehaviour
    {
        #region VARIABLES

        [Header("Character Creation Manager")]
        public CharacterCreationManager CreationManager;

        [Header("Material Image")]
        public Transform ColourSelectionParent;
        public MaterialImage MaterialImagePrefab;

        #endregion

        #region METHODS

        public virtual void FillPanel(List<Material> materials, Action<int> callback = null)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                callback?.Invoke(i);
            }
        }

        #endregion
    }
}
