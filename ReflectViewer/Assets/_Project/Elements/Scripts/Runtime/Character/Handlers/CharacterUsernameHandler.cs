using TMPro;
using Unity.Reflect.Viewer.UI;
using UnityEngine;

namespace Elements.Character
{
    public class CharacterUsernameHandler : MonoBehaviour
    {
        #region VARIABLES

        [Header("Character Data")]
        public CharacterData CharacterData;

        [Header("Username")]
        public TextMeshProUGUI UsernameText;

        #endregion

        #region UNITY METHODS

        #endregion

        #region METHODS

        public void UpdateUsername()
        {
            if (string.IsNullOrEmpty(CharacterData.Username) && UIStateManager.current)
                CharacterData.Username = UIStateManager.current.sessionStateData.sessionState.user.DisplayName;
            UsernameText.SetText(!string.IsNullOrEmpty(CharacterData.Username)
                                     ? CharacterData.Username
                                     : "Zutari.Guest-Player");
        }

        #endregion
    }
}
