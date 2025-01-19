using Hashira.Core.StatSystem;
using Hashira.Items;
using Hashira.Items.Weapons;
using System.Data;
using TMPro;
using UnityEngine;

namespace Hashira.UI.InGame
{
    public class ItemDataUI : MonoBehaviour
    {
        [SerializeField] protected SpriteRenderer[] _backgroundSprites;
        [SerializeField] protected SpriteRenderer _iconSprite;
        [SerializeField] protected TextMeshPro _itemNameText;
        [SerializeField] protected TextMeshPro _itemDescriptionText;
        [SerializeField] protected TextMeshPro _itemStatText;

        public void SetItem(Item item, IStatable statComparisonTarget = null)
        {
            _iconSprite.sprite = item.ItemSO.itemSprite;
            _itemNameText.text = item.ItemSO.itemDisplayName;
            _itemDescriptionText.text = item.ItemSO.itemDescription;

            int statCount = 0;
            string statStr = "";

            if (item is IStatable itemStat)
            {
                foreach (StatElement stat in itemStat.StatDictionary.GetElements())
                {
                    if (statComparisonTarget != null &&
                        statComparisonTarget.StatDictionary.TryGetElement(stat.elementSO, out StatElement comparisonStat))
                    {
                        bool isReverse = stat.elementSO.statName == "Speed";
                        bool isDefault = comparisonStat == null || comparisonStat.Value == stat.Value;
                        bool isDown = comparisonStat.Value > stat.Value;
                        statStr += isDefault ? "" : ((isDown ^ isReverse) ? "<color=red>" : "<color=yellow>");
                        statStr += $"{stat.elementSO.displayName} : {stat.Value}\n";
                        statStr += isDefault ? "" : "</color>";
                    }
                    else
                        statStr += $"{stat.elementSO.displayName} : {stat.Value}\n";
                }

                statCount = itemStat.StatDictionary.GetElements().Length;
            }

            _itemStatText.text = statStr;
            SetHeight(4.7f + statCount * 0.55f);
        }

        private void SetHeight(float height)
        {
            float yTop = height / 2;
            float yBottom = -height / 2;
            //Icon
            Vector3 iconPos = _iconSprite.transform.localPosition;
            iconPos.y = yTop - 1.2f;
            _iconSprite.transform.localPosition = iconPos;
            //Name
            Vector3 namePos = _itemNameText.transform.localPosition;
            namePos.y = yTop - 1.2f;
            _itemNameText.transform.localPosition = namePos;
            //Stat
            Vector3 statPos = _itemStatText.transform.localPosition;
            statPos.y = yTop - 1.95f;
            _itemStatText.transform.localPosition = statPos;

            //Description
            Vector3 descriptionPos = _itemDescriptionText.transform.parent.localPosition;
            descriptionPos.y = yBottom + 1.4f;
            _itemDescriptionText.transform.parent.localPosition = descriptionPos;

            foreach (SpriteRenderer sprite in _backgroundSprites)
            {
                Vector2 size = sprite.size;
                size.y = height;
                sprite.size = size;
            }
        }
    }
}
