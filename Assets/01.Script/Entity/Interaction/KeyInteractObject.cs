using Hashira.Entities.Components;
using UnityEngine;

namespace Hashira.Entities.Interacts
{
    public abstract class KeyInteractObject : MonoBehaviour, IInteractable
    {
        [SerializeField] protected GameObject _keyGuideObject;

        protected virtual void Awake()
        {
            _keyGuideObject.SetActive(false);
        }

        public virtual void Interaction(Entity entity)
        {

        }

        public virtual void OffInteractable()
        {
            _keyGuideObject.SetActive(false);
        }

        public virtual void OnInteractable()
        {
            _keyGuideObject.SetActive(true);
        }
    }
}
