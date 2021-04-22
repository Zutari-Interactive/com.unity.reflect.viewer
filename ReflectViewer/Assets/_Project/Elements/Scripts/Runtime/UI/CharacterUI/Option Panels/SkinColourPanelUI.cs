using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Elements.General;

namespace Elements.UI
{
    public class SkinColourPanelUI : ColourSelectionBaseUI
    {
        #region VARIABLES

        #endregion

        #region UNITY METHODS

        public void Start()
        {
            FillPanel(CreationManager.SkinMaterials, index =>
            {
                MaterialImage image = Instantiate(MaterialImagePrefab, ColourSelectionParent);
                image.MaterialIndex = index;
                image.ApplyImageColour(CreationManager.SkinMaterials[index].color, CharacterMaterial.Skin);
            });
        }

        #endregion

        #region METHODS

        #endregion
    }
}
