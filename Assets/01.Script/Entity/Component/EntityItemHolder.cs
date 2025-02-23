using Hashira.Core.EventSystem;
using Hashira.Items;
using Hashira.Items.Weapons;
using System;
using UnityEngine;

namespace Hashira.Entities.Components
{
    public abstract class EntityItemHolder : MonoBehaviour, IEntityComponent, IAfterInitialzeComponent, IEntityDisposeComponent
    {
        public Item CurrentItem
        {
            get => Items[CurrentIndex];
            protected set => Items[CurrentIndex] = value;
        }
        public Item[] Items { get; protected set; }

        [field: SerializeField] public Transform VisualTrm { get; private set; }

        public int CurrentIndex { get; protected set; } = 0;

        protected float _startYPos;
        protected SpriteRenderer _spriteRenderer;

        public Entity Entity { get; private set; }
        protected EntityMover _mover;
        protected EntitySoundGenerator _soundGenerator;

        public Action<Item>[] OnChangedItemEvents {  get; set; }
        public Action<Item> OnCurrentItemChanged { get; set; }

        public virtual void Initialize(Entity entity)
        {
            Entity = entity;
            _spriteRenderer = VisualTrm.GetComponent<SpriteRenderer>();
            CurrentIndex = 0;
            OnCurrentItemChanged += HandleChangedCurrentItem;

            _startYPos = transform.localPosition.y;

            _mover = entity.GetEntityComponent<EntityMover>(true);
			_soundGenerator = entity.GetEntityComponent<EntitySoundGenerator>(true);

		}

        public virtual void AfterInit()
        {

        }

        protected virtual void HandleChangedCurrentItem(Item item)
        {
            _spriteRenderer.sprite = item?.ItemSO.itemDefaultSprite;
        }

        public virtual void RemoveWeapon(int index)
        {
            Items[index]?.UnEquip();
            Items[index] = null;

            for (int i = 0; i < Items.Length; i++)
            {
                CurrentIndex++;
                if (CurrentIndex >= Items.Length) CurrentIndex = 0;
                if (Items[CurrentIndex] != null) break;
            }
            OnChangedItemEvents[index]?.Invoke(null);
        }

        public virtual T EquipItem<T>(T item, int index) where T : Item
        {
            int itemIndex = GetIndex(index);
            if (index == -1) CurrentIndex = itemIndex;
            T prevGunWeapon = CurrentItem as T;

            CurrentItem?.UnEquip();
            CurrentItem = item;
            CurrentItem?.Equip(this);

            OnChangedItemEvents[CurrentIndex]?.Invoke(item);
            OnCurrentItemChanged?.Invoke(item);
            return prevGunWeapon;
        }

        protected int GetIndex(int index)
        {
            if (index == -1)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    if (Items[i] == null)
                        return i;
                }
                return CurrentIndex;
            }
            else
                return index >= Items.Length ? Items.Length - 1 : index;
        }

        public void WeaponChange(int index)
        {
            if (index >= Items.Length)
                index = Items.Length - 1;

            CurrentIndex = index;
            OnCurrentItemChanged?.Invoke(CurrentItem);
        }

        public void WeaponSwap()
        {
            for (int i = 0; i < Items.Length; i++)
            {
                CurrentIndex++;
                if (CurrentIndex >= Items.Length) CurrentIndex = 0;
                if (CurrentItem != null) break;
            }

            OnCurrentItemChanged?.Invoke(CurrentItem);
        }

        protected virtual void Update()
        {
            CurrentItem?.ItemUpdate();
        }

        public virtual void Dispose()
        {
            OnCurrentItemChanged -= HandleChangedCurrentItem;
        }
    }
}
