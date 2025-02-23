using Hashira.Core;
using Hashira.Core.StatSystem;
using Hashira.Items;
using Hashira.Items.PartsSystem;
using Hashira.Items.SubItems;
using Hashira.Items.Weapons;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hashira.UI.InGame
{
    public class ItemDataUI : MonoBehaviour
    {
        [SerializeField] protected Image _iconSprite;
        [SerializeField] protected TextMeshProUGUI _itemNameText;
        [SerializeField] protected TextMeshProUGUI _itemTypeText;
        [SerializeField] protected TextMeshProUGUI _itemDescriptionText;
        [SerializeField] protected TextMeshProUGUI _itemStatText;
        [SerializeField] protected Transform _equipableGunImageTrm;
        [SerializeField] protected Image _equipableGunImage;

        public void SetItem(Item item, IStatable statComparisonTarget = null)
        {
            for (int i = 0; i < _equipableGunImageTrm.childCount; i++)
                Destroy(_equipableGunImageTrm.GetChild(i).gameObject);

            _iconSprite.sprite = item.ItemSO.itemDefaultSprite;
            _itemNameText.text = item.ItemSO.itemDisplayName;
            _itemDescriptionText.text = item.ItemSO.itemDescription;
            if (item is GunWeapon gunWeapon)
                _itemTypeText.text = "원거리무기";
            else if (item is SubItem subItem)
                _itemTypeText.text = "보조 아이템";
            else if (item is WeaponParts weaponParts)
            {
                foreach (var gunSO in weaponParts.WeaponPartsSO.partsSpriteDictionary.Keys)
                {
                    Image image = Instantiate(_equipableGunImage, _equipableGunImageTrm);
                    image.sprite = gunSO.itemIcon;
                }
                _itemTypeText.text = EnumUtility.WeaponPartsTypeNameDict[weaponParts.WeaponPartsSO.partsType];
            }


            string statStr = "";

            if (item is IStatable itemStat)
            {
                foreach (StatElement stat in itemStat.StatDictionary.GetElements())
                {
                    if (statComparisonTarget != null &&
                        statComparisonTarget.StatDictionary.TryGetElement(stat.elementSO, out StatElement comparisonStat))
                    {
                        bool isPreferLow = stat.elementSO.isPreferLow;
                        bool isDefault = comparisonStat == null || comparisonStat.Value == stat.Value;
                        bool isDown = comparisonStat.Value > stat.Value;
                        statStr += isDefault ? "" : ((isDown ^ isPreferLow) ? "<color=red>" : "<color=yellow>");
                        statStr += $"{stat.elementSO.displayName} : {stat.Value}\n";
                        statStr += isDefault ? "" : "</color>";
                    }
                    else
                        statStr += $"{stat.elementSO.displayName} : {stat.Value}\n";
                }
            }

            _itemStatText.text = statStr;
        }
    }
}
