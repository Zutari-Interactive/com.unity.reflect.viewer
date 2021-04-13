using UnityEngine;
using Zutari.General;

namespace Zutari.Character
{
    public class CharacterEntity : EntityBase
    {
        #region VARIABLES

        [Header("Handlers")]
        public CharacterMaterialHandler MaterialHandler;
        public CharacterUsernameHandler UsernameHandler;

        #endregion

        #region UNITY METHODS

        public void Awake()
        {
            DoOnAwake();
        }

        #endregion

        #region METHODS

        #endregion

        #region OVERRIDE METHODS

        public override void DoOnAwake()
        {
            UsernameHandler.UpdateUsername();
        }

        #endregion
    }
}
