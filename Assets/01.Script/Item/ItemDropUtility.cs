using System;
using System.Collections.Generic;
using Crogen.CrogenPooling;
using Hashira.Entities.Interacts;
using Hashira.Items.WeaponPartsSystem;
using Hashira.Items.Weapons;
using UnityEditor;
using UnityEngine;

namespace Hashira.Items
{
    public class ItemDropUtility : MonoBehaviour
    {
        [SerializeField] private ItemSO[] _items;

        private static Dictionary<Type, ItemSO> _itemDict;

        private static GameObject _gameObject;

#if UNITY_EDITOR
        private void Reset()
        {
            string[] guids = AssetDatabase.FindAssets("t:ItemSO");
            List<ItemSO> itemList = new List<ItemSO>();
            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                var so = AssetDatabase.LoadAssetAtPath<ItemSO>(path);
                itemList.Add(so);
            }
            _items = itemList.ToArray();
        }
#endif
        
        private void Awake()
        {
            _itemDict = new Dictionary<Type, ItemSO>();

            foreach (ItemSO item in _items)
            {
                _itemDict.Add(item.GetType(), item);
            }
            
            _gameObject = gameObject;
        }

        public static DroppedWeapon DropWeaponItem<T>(Vector2 position) where T : WeaponSO
        {
            var weaponItem = _gameObject.Pop(ItemPoolType.WeaponItem, position, Quaternion.identity) as DroppedWeapon;
            if (weaponItem != null) weaponItem.SetWeapon((_itemDict[typeof(T)] as WeaponSO)?.GetWeaponClass());
            return weaponItem;
        }

        public static DroppedParts DropPartsItem<T>(Vector2 position) where T : WeaponPartsSO
        {
            var weaponPartItem = _gameObject.Pop(ItemPoolType.WeaponPartsItem, position, Quaternion.identity) as DroppedParts;
            if (weaponPartItem != null) weaponPartItem.SetParts((_itemDict[typeof(T)] as WeaponPartsSO)?.GetWeaponPartsClass());
            return weaponPartItem;
        }
    }
}
