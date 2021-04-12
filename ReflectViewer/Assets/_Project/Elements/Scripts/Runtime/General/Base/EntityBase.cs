using UnityEngine;

namespace Zutari.General
{
    public abstract class EntityBase : MonoBehaviour
    {
        #region METHODS

        public virtual void Activate()
        {
            gameObject.SetActive(true);
        }

        public virtual void Deactivate()
        {
            gameObject.SetActive(false);
        }

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }

        #endregion

        #region VIRTUAL METHODS

        public virtual void DoOnAwake()
        {
        }

        public virtual void DoOnEnable()
        {
        }

        public virtual void DoOnDisable()
        {
        }

        public virtual void DoOnDestroy()
        {
        }

        #endregion
    }
}
