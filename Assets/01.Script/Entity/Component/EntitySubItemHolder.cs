using Hashira.Items;
using Hashira.Items.SubItems;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public class EntitySubItemHolder : EntityItemHolder
    {
        public SubItem CurrentSubItem
        {
            get => SubItems[CurrentIndex];
            protected set => SubItems[CurrentIndex] = value;
        }
        public SubItem[] SubItems { get; protected set; }

        [SerializeField] private SubItemSO[] _defaultWeapons;

        [field: SerializeField] public DamageCaster2D DamageCaster { get; private set; }

        public EntityWeaponHolder GunWaepon { get; private set; }

        private float _meleeAttackCooltime = 0.5f;
        private float _lastMeleeAttackTime;

        public bool IsCharged { get; private set; }

        public Action<SubItem>[] OnChangedSubItemEvents;
        public Action<SubItem> OnCurrentSubItemChanged;

        public override void Initialize(Entity entity)
        {
            base.Initialize(entity);

            Items = new Item[_defaultWeapons.Length];

            OnChangedItemEvents = new Action<Item>[_defaultWeapons.Length];
            OnChangedSubItemEvents = new Action<SubItem>[_defaultWeapons.Length];
            for (int i = 0; i < OnChangedItemEvents.Length; i++)
            {
                int index = i;
                OnChangedItemEvents[index] += item => OnChangedSubItemEvents[index]?.Invoke(item as SubItem);
            }
            OnCurrentItemChanged += item => OnCurrentSubItemChanged?.Invoke(item as SubItem);

            GunWaepon = entity.GetEntityComponent<EntityWeaponHolder>();
        }

        public override void AfterInit()
        {
            base.AfterInit();
            SubItems = new SubItem[_defaultWeapons.Length];
            for (int i = 0; i < _defaultWeapons.Length; i++)
            {
                if (_defaultWeapons[i] == null) continue;
                EquipItem(_defaultWeapons[i].GetItemClass() as SubItem, i);
            }

            OnCurrentSubItemChanged?.Invoke(CurrentSubItem);
        }

        public override void RemoveWeapon(int index)
        {
            base.RemoveWeapon(index);

            if (index == CurrentIndex)
            {
                OnCurrentSubItemChanged?.Invoke(SubItems[index]);

                if (GunWaepon != null)
                    GunWaepon.EquipItem(GunWaepon.CurrentItem as GunWeapon, GunWaepon.CurrentIndex);
            }
        }

        public override T EquipItem<T>(T item, int index = -1)
        {
            if (GunWaepon == null && (index == CurrentIndex || index == -1))
                OnCurrentSubItemChanged?.Invoke(item as SubItem);

            return base.EquipItem(item, index);
        }

        public virtual void Use()
        {
            if (CurrentSubItem == null) return;
            if (_lastMeleeAttackTime + _meleeAttackCooltime > Time.time) return;

            _lastMeleeAttackTime = Time.time;

            HandleChangedCurrentItem(CurrentSubItem);

            if (GunWaepon != null)
                GunWaepon.IsSubItemMode = true;
        }

        //public void ChargeAttack(int damage, bool isDown, LayerMask whatIsTarget)
        //{
        //    if (CurrentWeapon == null) return;

        //    HandleChangedCurrentWeapon(CurrentWeapon);

        //    if (GunWaepon != null)
        //        GunWaepon.IsSubItemMode = true;

        //    IsCharged = true;
        //    base.Attack(damage, isDown, whatIsTarget);
        //    IsCharged = false;
        //}
    }
}
