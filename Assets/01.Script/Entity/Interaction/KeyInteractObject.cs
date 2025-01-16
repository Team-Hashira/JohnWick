using Hashira.Entities.Components;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace Hashira.Entities.Interacts
{
    public abstract class KeyInteractObject : MonoBehaviour, IInteractable
    {
        [SerializeField] protected GameObject _keyGuideObject;
        [SerializeField] protected TMP_Text _keyText, _nameText;
        [SerializeField] protected InputReaderSO _inputReader;

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
            _keyText.text = _inputReader.InteractKey;
            _keyGuideObject.SetActive(true);
        }
    }
}
