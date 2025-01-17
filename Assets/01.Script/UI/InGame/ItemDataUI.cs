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
            Vector3 iconPos = _iconSprite.transform.position;
            iconPos.y = yTop - 1.1f;
            _iconSprite.transform.position = iconPos;
            //Name
            Vector3 namePos = _itemNameText.transform.position;
            namePos.y = yTop - 1.1f;
            _itemNameText.transform.position = namePos;
            //Stat
            Vector3 statPos = _itemStatText.transform.position;
            statPos.y = yTop - 1.9f;
            _itemStatText.transform.position = statPos;

            //Description
            Vector3 descriptionPos = _itemDescriptionText.transform.parent.position;
            descriptionPos.y = yBottom + 1.6f;
            _itemDescriptionText.transform.parent.position = descriptionPos;

            foreach (SpriteRenderer sprite in _backgroundSprites)
            {
                Vector2 size = sprite.size;
                size.y = height;
                sprite.size = size;
            }
        }
    }
}
