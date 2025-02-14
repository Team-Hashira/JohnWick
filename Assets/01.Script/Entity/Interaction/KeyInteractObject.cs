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
        protected readonly static int _FillAmountShaderHash = Shader.PropertyToID("_FillAmount");

        [Header("==========KeyInteractObject setting==========")]
        [SerializeField] protected GameObject _keyGuideObject;
        [SerializeField] protected TMP_Text _keyText, _nameText;
        [SerializeField] protected InputReaderSO _inputReader;
        [SerializeField] protected SpriteRenderer _itemSprite;
        [SerializeField] protected SpriteRenderer _holdOutlineSprite;

        protected Material _holdOutlineMat;

        protected virtual void Awake()
        {
            _keyGuideObject.SetActive(false);
            _holdOutlineMat = _holdOutlineSprite.material;
            _holdOutlineMat.SetFloat(_FillAmountShaderHash, 0);
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
