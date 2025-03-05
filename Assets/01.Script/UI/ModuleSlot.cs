//using Hashira.Items.Modules;
//using Hashira.UI.DragSystem;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//namespace Hashira.UI
//{
//    public class ModuleSlot : MonoBehaviour, ISelectableObject
//    {
//        [SerializeField] private Image _iconImage;
//        [SerializeField] private TextMeshProUGUI _countText;
//        CardSO _baseModuleSO;

//        public void Init(CardSO baseModule, int count = 1)
//        {
//            _baseModuleSO = baseModule;
//            _iconImage.sprite = baseModule.card;
//            if (count == 1)
//                _countText.text = string.Empty;
//            else
//                _countText.text = count.ToString();
//        }

//        public void OnSelectStart()
//        {

//        }

//        public void OnSelectEnd()
//        {

//        }
//    }
//}
