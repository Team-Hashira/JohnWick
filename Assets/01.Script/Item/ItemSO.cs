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

        [SerializeReference] protected Item _itemClass;

        public Item GetItemClass()
            => _itemClass?.Clone() as Item;

        protected virtual void OnValidate()
        {
            if (useCustomClass == false && defaultClass == "") return;

            string thisTag = GetType().ToString().Replace("SO", "");
            int tagStartIdx = thisTag.LastIndexOf(".");                 //Namespace.Class
            string namespaceName = thisTag[..tagStartIdx];              //Namespace
            string typeName;
            string tagName = thisTag[++tagStartIdx..];
            if (useCustomClass)
            {             //Class
                typeName = $"{namespaceName}.{itemName}{tagName}";    //Namespace.Item
            }
            else
            {
                typeName = $"{namespaceName}.{defaultClass}";          //Namespace.Default
            }

            if (_itemClass != null && _itemClass.ToString() == typeName) return;


            try
            {
                Type type = Type.GetType(typeName);
                Item item = Activator.CreateInstance(type) as Item;
                item.Init(this);
                _itemClass = item;
            }
            catch (Exception ex)
            {
                _itemClass = null;
                Debug.LogError($"{typeName} not found.\n" +
                                $"Error : {ex.ToString()}");
            }
        }
    }
}
