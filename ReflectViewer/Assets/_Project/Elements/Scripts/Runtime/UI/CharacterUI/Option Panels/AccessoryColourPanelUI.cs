using Zutari.General;

namespace Zutari.UI
{
    public class AccessoryColourPanelUI : ColourSelectionBaseUI
    {
        #region VARIABLES

        #endregion

        #region UNITY METHODS

        public void Start()
        {
            FillPanel(CreationManager.AccessoryMaterials, index =>
            {
                MaterialImage image = Instantiate(MaterialImagePrefab, ColourSelectionParent);
                image.MaterialIndex = index;
                image.ApplyImageColour(CreationManager.AccessoryMaterials[index].color, CharacterMaterial.Accessory);
            });
        }

        #endregion

        #region METHODS

        #endregion
    }
}
