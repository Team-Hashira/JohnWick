using Doryu.CustomAttributes;
using System;
using UnityEngine;

namespace Hashira.Items
{
    public enum EItemRank
    {
        Common,
        UnCommon,
        Rare,
        Epic,
        Legendary,
    }

    public abstract class ItemSO : ScriptableObject
    {
        [Header("==========Item setting==========")]
        [Tooltip("UI에 사용하는거")]
        public Sprite itemIcon;
        [Tooltip("인게임에 사용하는거")]
        public Sprite itemDefaultSprite;
        [Tooltip("영어명")]
        public string itemName;
        [Tooltip("한글명")]
        public string itemDisplayName;
        [TextArea]
        public string itemDescription;
        public EItemRank itemRating;

        public bool useCustomClass;
        [ToggleField("useCustomClass", false)]
        public string defaultClass;

        protected Item _itemClass;

        public Item GetItemClass()
            => _itemClass?.Clone() as Item;

        protected virtual void OnEnable()
        {
            if (useCustomClass == false && defaultClass == "") return;

            string thisTag = GetType().ToString().Replace("SO", "");
            int tagStartIdx = thisTag.LastIndexOf(".");                 //Namespace.Class
            string namespaceName = thisTag[..tagStartIdx];              //Namespace
            string typeName;
            if (useCustomClass)
            {
                string tagName = thisTag[++tagStartIdx..];              //Class
                typeName = namespaceName + "." + itemName + tagName;    //Namespace.Item
            }
            else
            {
                typeName = namespaceName + "." + defaultClass;          //Namespace.Default
            }

            try
            {
                Type type = Type.GetType(typeName);
                Item foundWeapon = Activator.CreateInstance(type) as Item;
                foundWeapon.Init(this);
                _itemClass = foundWeapon;
            }
            catch (Exception ex)
            {
                Debug.LogError($"{typeName} not found.\n" +
                                $"Error : {ex.ToString()}");
            }
        }
    }
}
