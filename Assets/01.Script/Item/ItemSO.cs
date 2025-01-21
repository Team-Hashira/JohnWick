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

        public bool useCustomClass;
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
