using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zutari.General;

namespace Zutari.UI
{
    public class ClothsColourPanelUI : ColourSelectionBaseUI
    {
        #region VARIABLES

        #endregion

        #region UNITY METHODS

        public void Start()
        {
            FillPanel(CreationManager.ClothesMaterials, index =>
            {
                MaterialImage image = Instantiate(MaterialImagePrefab, ColourSelectionParent);
                image.MaterialIndex = index;
                image.ApplyImageColour(CreationManager.ClothesMaterials[index].color, CharacterMaterial.Clothes);
            });
        }

        #endregion

        #region METHODS

        #endregion
    }
}
