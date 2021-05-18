using UnityEngine;

namespace Elements.Character
{
    public class CharacterMaterialHandler : MonoBehaviour
    {
        #region VARIABLES

        [Header("Renderers")]
        public Renderer SkinRenderer;
        public Renderer ClothRenderer;
        public Renderer AccessoryRenderer;

        private Material[] _sharedMaterials;

        #endregion

        #region METHODS

        public void ChangeSkinColour(Material material)
        {
            _sharedMaterials = SkinRenderer.sharedMaterials;
            _sharedMaterials[(int) CharacterMaterial.Skin] = material;
            SkinRenderer.sharedMaterials = _sharedMaterials;
        }

        public void ChangeClothColour(Material material)
        {
            _sharedMaterials = ClothRenderer.sharedMaterials;
            _sharedMaterials[(int) CharacterMaterial.Clothes] = material;
            ClothRenderer.sharedMaterials = _sharedMaterials;
        }

        public void ChangeAccessoryColour(Material material)
        {
            _sharedMaterials = AccessoryRenderer.sharedMaterials;
            _sharedMaterials[(int) CharacterMaterial.Accessory] = material;
            AccessoryRenderer.sharedMaterials = _sharedMaterials;
        }

        #endregion
    }
}
