using Hashira.Core.StatSystem;
using Hashira.Entities.Components;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Items.PartsSystem
{
    public class WeaponParts : Item
    {
        public PartsSO WeaponPartsSO { get; private set; }

        public override StatDictionary StatDictionary => WeaponPartsSO.StatDictionary;
         
        protected GunWeapon _weapon;

        protected PartsRenderer _PartsRenderer { get; private set; }
        protected Transform transform;

        //���� ó�� ������� �� �ѹ�
        public override void Init(ItemSO itemSO)
        {
            base.Init(itemSO);

            WeaponPartsSO = itemSO as PartsSO;
        }

        public virtual void Equip(GunWeapon weapon)
        {
            _weapon = weapon;
            _weapon.OnPartsRendererChangedEvent += HandlePartsRendererChangedEvent;
            HandlePartsRendererChangedEvent(weapon.PartsRenderer);

            foreach (StatElement stat in WeaponPartsSO.StatDictionary.GetElements())
                _weapon.StatDictionary[stat.elementSO].AddModify(WeaponPartsSO.itemName, stat.Value, EModifyMode.Add);
            Debug.Log($"{WeaponPartsSO.itemDisplayName} Equip!");
        }

        protected virtual void HandlePartsRendererChangedEvent(PartsRenderer renderer)
        {
            _PartsRenderer = renderer;
            transform = renderer?[WeaponPartsSO.partsType].transform;
        }

        public override void UnEquip()
        {
            _weapon.OnPartsRendererChangedEvent -= HandlePartsRendererChangedEvent;

            foreach (StatElement stat in WeaponPartsSO.StatDictionary.GetElements())
                _weapon.StatDictionary[stat.elementSO].RemoveModify(WeaponPartsSO.itemName, EModifyMode.Add);
            Debug.Log($"{WeaponPartsSO.itemDisplayName} UnEquip!");
        }
        public virtual void PartsUpdate()
        {

        }

        //여기 해야해
        public override void Equip(EntityItemHolder holder)
        {
            throw new NotImplementedException();
        }

        public override void ItemUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
