using Hashira.Core.StatSystem;
using Hashira.Items.PartsSystem;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Items
{
    public abstract class ItemSO : ScriptableObject
    {
        [Header("==========Item setting==========")]
        public Sprite itemIcon;
        public Sprite itemSprite;
        [Tooltip("영어명")]
        public string itemName;
        [Tooltip("한글명")]
        public string itemDisplayName;
        [TextArea]
        public string itemDescription;

        public bool useItemClass;
        public string isUseItemClass;
        protected Item _itemClass;

        public Item GetItemClass()
        {
            if (_itemClass != null)
                return _itemClass.Clone() as Item;
            else
                return null;
        }

        private void OnEnable()
        {
            string thisTag = GetType().ToString().Replace("SO", "");
            int tagStartIdx = thisTag.LastIndexOf(".");                 //Namespace.Class
            string namespaceName = thisTag[..tagStartIdx];              //Namespace
            string tagName = thisTag[++tagStartIdx..];                  //Class
            string typeName = namespaceName + "." + itemName + tagName;  //Namespace.Item
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
