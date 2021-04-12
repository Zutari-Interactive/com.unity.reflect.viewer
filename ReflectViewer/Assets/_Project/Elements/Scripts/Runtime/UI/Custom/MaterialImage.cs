using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zutari.General;
using Zutari.Managers;

namespace Zutari.UI
{
    public class MaterialImage : MonoBehaviour, IPointerClickHandler
    {
        #region VARIABLES

        [Header("Image")]
        public Image Image;

        [Header("Material")]
        public int MaterialIndex = -1;
        public CharacterMaterial CharacterMaterial;

        #endregion

        #region UNITY METHODS

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                if (CharacterMaterial == CharacterMaterial.Skin)
                    CharacterCreationManager.OnChangeSkinMaterial?.Invoke(MaterialIndex);
                if (CharacterMaterial == CharacterMaterial.Clothes)
                    CharacterCreationManager.OnChangeClothMaterial?.Invoke(MaterialIndex);
                if (CharacterMaterial == CharacterMaterial.Accessory)
                    CharacterCreationManager.OnChangeAccessoryMaterial?.Invoke(MaterialIndex);
            }
        }

        #endregion

        #region METHODS

        public void ApplyImageColour(Color color, CharacterMaterial characterMaterial)
        {
            CharacterMaterial = characterMaterial;
            Image.color = color;
        }

        #endregion
    }
}
