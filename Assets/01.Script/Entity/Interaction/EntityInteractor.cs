using Hashira.Entities.Interacts;
using UnityEngine;

namespace Hashira.Entities
{
    public class EntityInteractor : MonoBehaviour, IEntityComponent
    {
        public bool CanInteract { get; private set; }
        public IInteractable Interactable { get; private set; }
        public IHoldInteractable HoldInteractable { get; private set; }

        [SerializeField] private InputReaderSO _input;
        [SerializeField] private LayerMask _interactable;
        [SerializeField] private float _radius;

        private Entity _entity;
        private Collider2D[] collider2Ds = new Collider2D[1];

        private bool _isClicked = false;
        private bool _isHolding = false;
        private float _holdTime = 0.2f;
        private float _holdStartTime = 0;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        public void Interact(bool isDown)
        {
            if (Interactable == null) return;
            if (_isClicked == isDown) return;

            _isClicked = isDown;
            if (isDown)
                _holdStartTime = Time.time;
            else
            {
                if (_isHolding == false)
                    Interactable?.Interaction(_entity);
                else
                {
                    _isHolding = false;
                    HoldInteractable?.HoldInteractionEnd();
                }
            }
        }

        private void TargetInteractableUpdate()
        {
            collider2Ds = Physics2D.OverlapCircleAll(transform.position, _radius, _interactable);
            IInteractable interactable 
                = collider2Ds.Length > 0 ? collider2Ds[0].GetComponent<IInteractable>() : null;

            if (Interactable == interactable) return;
            else
            {
                _isClicked = false;
                _isHolding = false;
            }

            Interactable?.OffInteractable();
            HoldInteractable?.HoldInteractionEnd();
            Interactable = interactable;
            HoldInteractable = interactable as IHoldInteractable;
            Interactable?.OnInteractable();
        }

        private void Update()
        {
            TargetInteractableUpdate();

            if (_isClicked && _holdStartTime + _holdTime < Time.time && _isHolding == false)
            {
                _isHolding = true;
                HoldInteractable?.HoldInteractionStart(_entity);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _radius);
        }
    }
}
