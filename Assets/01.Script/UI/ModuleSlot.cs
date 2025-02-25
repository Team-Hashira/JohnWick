using Hashira.Items.Modules;
using Hashira.UI.DragSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI
{
    public class ModuleSlot : MonoBehaviour, ISelectableObject
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _countText;
        ModuleSO _baseModuleSO;

        public void Init(ModuleSO baseModule, int count = 1)
        {
            _baseModuleSO = baseModule;
            _iconImage.sprite = baseModule.itemIcon;
            if (count == 1)
                _countText.text = string.Empty;
            else
                _countText.text = count.ToString();
        }

        public void OnSelectStart()
        {

        }

        public void OnSelectEnd()
        {

        }
    }
}
