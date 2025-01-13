using Hashira.Entities.Interacts;
using UnityEngine;

namespace Hashira.Entities
{
    public class EntityInteractor : MonoBehaviour, IEntityComponent
    {
        public bool CanInteract { get; private set; }
        public IInteractable Interactable { get; private set; }

        [SerializeField] private InputReaderSO _input;
        [SerializeField] private LayerMask _interactable;
        [SerializeField] private float _radius;

        private Entity _entity;
        private Collider2D[] collider2Ds = new Collider2D[1];

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void Interact()
        {
            Interactable?.Interaction(_entity);
        }

        private void TargetInteractableUpdate()
        {
            collider2Ds = Physics2D.OverlapCircleAll(transform.position, _radius, _interactable);
            IInteractable interactable 
                = collider2Ds.Length > 0 ? collider2Ds[0].GetComponent<IInteractable>() : null;
            Interactable?.OffInteractable();
            Interactable = interactable;
            Interactable?.OnInteractable();
        }

        private void Update()
        {
            TargetInteractableUpdate();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
