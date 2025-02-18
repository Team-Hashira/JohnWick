using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.InGame
{
    public class ItemDataUIController : MonoBehaviour, IPopupUI
    {
        public EPopupUIName PopupUIName { get; } = EPopupUIName.ItemDataUI;

        [SerializeField] private ItemDataUI _leftItemDataUI;
        [SerializeField] private Image _arrowImage;
        [SerializeField] private ItemDataUI _rightItemDataUI;

        private void Start()
        {
            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }
        public void SetItem(Weapon weapon, Weapon myWeapon = null)
        {
            if (myWeapon != null)
            {
                _leftItemDataUI.SetItem(myWeapon);
                _rightItemDataUI.SetItem(weapon, myWeapon);

                _rightItemDataUI.transform.gameObject.SetActive(true);
                _arrowImage.transform.gameObject.SetActive(true);
            }
            else
            {
                _leftItemDataUI.SetItem(weapon);

                _rightItemDataUI.transform.gameObject.SetActive(false);
                _arrowImage.transform.gameObject.SetActive(false);
            }
        }
        public void SetItem(WeaponParts parts, WeaponParts myParts = null)
        {
            if (myParts != null)
            {
                _leftItemDataUI.SetItem(myParts);
                _rightItemDataUI.SetItem(parts, myParts);

                _rightItemDataUI.transform.gameObject.SetActive(true);
                _arrowImage.transform.gameObject.SetActive(true);
            }
            else
            {
                _leftItemDataUI.SetItem(parts);

                _rightItemDataUI.transform.gameObject.SetActive(false);
                _arrowImage.transform.gameObject.SetActive(false);
            }
        }
    } 
}
